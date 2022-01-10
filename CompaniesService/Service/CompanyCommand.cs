using AutoMapper;
using ConstData;
using NHibernate.Linq;
using ServicesInterfaces.Companies;
using System;
using System.Threading.Tasks;
using TestBase;
using TestBase.Data;
using TestBaseDto.Company;

namespace CompaniesService.Service
{
    public class CompanyCommand: ICompanyCommand
    {
        private readonly INHibernateSession _session;
        private readonly IMapper _mapper;

        public CompanyCommand(INHibernateSession session, 
            IMapper mapper)
        {
            _session = session;
            _mapper = mapper;
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
    }
}