using System.Collections.Generic;
using System.Threading.Tasks;
using TestBaseDto.Company;

namespace ServicesInterfaces.Companies
{
    //Interfaces for read data from database
    public interface ICompanyQuery
    {
        Task<IEnumerable<CompanyAllDataDto>> GetCompaniesAsync();
        Task<CompanyAllDataDto> GetCompanyAsync(int id);
        Task<IEnumerable<CompanyAllDataDto>> GetCompanyWithCountUsersAsync(int count);
    }
}
