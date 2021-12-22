using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ConstData;
using MassTransit;
using NHibernate.Linq;
using ServicesInterfaces;
using TestBase;
using TestBase.Data;
using TestBaseDto;

namespace Services.Company
{
    public class CompanyService: ICompanyService
    {
        private readonly INHibernateSession _session;
        private readonly IMapper _mapper;

        public CompanyService(INHibernateSession session, 
            IMapper mapper, 
            IPublishEndpoint publishEndpoint)
        {
            _session = session;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CompanyDto>> GetCompaniesAsync()
        {
            var companies = await _session.Companies.ToListAsync();
            return _mapper.Map<List<TestBase.Data.Company>, IEnumerable<CompanyDto>>(companies);
        }

        public async Task<CompanyDto> GetCompanyAsync(int id)
        {
            var company = await _session.Companies.FirstOrDefaultAsync(c => c.Id == id);

            if (company == null)
                throw new ServiceException(ConstStringForException.NotFoundCompany);
            
            return _mapper.Map<CompanyDto>(company);
        }

        public async Task UpdateCompanyAsync(CompanyDto company)
        {
            try
            {
                _session.BeginTransaction();
                var updateCompany = _mapper.Map<TestBase.Data.Company>(company);
                 
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
                    throw new ServiceException(ConstStringForException.NotFoundCompany);

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
               
                var createCompany = _mapper.Map<TestBase.Data.Company>(company);
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
            var companies = await _session.Companies.Where(c => c.Users.Count >= count)
                .ToListAsync();
            return _mapper.Map<List<TestBase.Data.Company>, IEnumerable<CompanyDto>>(companies);
        }
    }
}