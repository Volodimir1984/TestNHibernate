using System.Collections.Generic;
using System.Threading.Tasks;
using TestBaseDto;

namespace ServicesInterfaces
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyDto>> GetCompaniesAsync();
        Task<CompanyDto> GetCompanyAsync(int id);
        Task UpdateCompanyAsync(CompanyDto company);
        Task DeleteCompanyAsync(int companyId);
        Task CreateCompanyAsync(CompanyDto company);
        Task<IEnumerable<CompanyDto>> GetCompanyWithCountUsersAsync(int count);
    }
}