using System.Threading.Tasks;
using TestBaseDto.Company;

namespace ServicesInterfaces.Companies
{
    public interface ICompanyCommand
    {
        Task UpdateCompanyAsync(CompanyDto company);
        Task DeleteCompanyAsync(int companyId);
        Task CreateCompanyAsync(CompanyDto company);
    }
}