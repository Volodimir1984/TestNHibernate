using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ConstData;
using Microsoft.Extensions.Caching.Distributed;
using NHibernate.Linq;
using ServicesInterfaces.Companies;
using TestBase;
using TestBase.Data;
using TestBaseDto;

namespace CompaniesService.Service
{
    public class CompanyService: ICompanyService
    {
        private readonly INHibernateSession _session;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        public CompanyService(INHibernateSession session, 
            IMapper mapper, 
            IDistributedCache cache)
        {
            _session = session;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<IEnumerable<CompanyDto>> GetCompaniesAsync()
        {
            var key = "Companies";
            var companiesDto = await _cache.GetCacheByKeyAsync<IEnumerable<CompanyDto>>(key);

            if (companiesDto == null)
            {
                var companies = await _session.Companies.ToListAsync();
                companiesDto = _mapper.Map<IEnumerable<Company>, IEnumerable<CompanyDto>>(companies);
                await _cache.SetCacheAsync(key, companiesDto);
            }

            return companiesDto;
        }

        public async Task<CompanyDto> GetCompanyAsync(int id)
        {
            var key = "Company:" + id;

            var companyDto = await _cache.GetCacheByKeyAsync<CompanyDto>(key);

            if (companyDto == null)
            {
                var company = await _session.Companies.FirstOrDefaultAsync(c => c.Id == id);

                if (company == null)
                    throw new CompanyServiceException(ConstStringForException.NotFoundCompany);

                companyDto = _mapper.Map<CompanyDto>(company);
                await _cache.SetCacheAsync(key, companyDto);
            }

            return companyDto;
        }

        public async Task UpdateCompanyAsync(CompanyDto company)
        {
            try
            {
                _session.BeginTransaction();
                var updateCompany = _mapper.Map<Company>(company);
                 
                await _session.SaveAsync(updateCompany);
                await _session.CommitAsync();
            }
            catch (Exception e)
            {
                try
                {
                    await _session.RollbackAsync();
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                throw new Exception(e.Message);
            }
            finally
            {
                _session.CloseTransaction();
            }
        }

        public async Task DeleteCompanyAsync(int companyId)
        {
            try
            {
                _session.BeginTransaction();
                var deleteCompany = await _session.Companies.FirstOrDefaultAsync(c => c.Id == companyId);

                if (deleteCompany == null)
                    throw new CompanyServiceException(ConstStringForException.NotFoundCompany);

                await _session.DeleteAsync(deleteCompany);
                await _session.CommitAsync();
            }
            catch(Exception e)
            {
                try
                {
                    await _session.RollbackAsync();
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                throw new Exception(e.Message);
            }
            finally
            {
                _session.CloseTransaction();
            }
        }

        public async Task CreateCompanyAsync(CompanyDto company)
        {
            try
            {
                _session.BeginTransaction();
               
                var createCompany = _mapper.Map<Company>(company);
                createCompany.Users = new List<AspNetUsers>();

                foreach (var user in company.Users)
                {
                    var u = new AspNetUsers
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Company = createCompany,
                    };
                    
                    createCompany.Users.Add(u);
                }
                
                await _session.SaveAsync(createCompany);
                await _session.CommitAsync();
            }
            catch (Exception e)
            {
                try
                {
                    await _session.RollbackAsync();
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                throw new Exception(e.Message);
            }
            finally
            {
                _session.CloseTransaction();
            }
        }

        public async Task<IEnumerable<CompanyDto>> GetCompanyWithCountUsersAsync(int count)
        {
            var key = "CompanyWithCountUsers:" + count;

            var companiesDto = await _cache.GetCacheByKeyAsync<IEnumerable<CompanyDto>>(key);

            if (companiesDto == null)
            {
                var companies = await _session.Companies.Where(c => c.Users.Count >= count)
                    .ToListAsync();
                companiesDto = _mapper.Map<List<Company>, IEnumerable<CompanyDto>>(companies);
                await _cache.SetCacheAsync(key, companiesDto);
            }

            return companiesDto;
        }
    }
}