using System.Threading.Tasks;
using TestBaseDto.Company;

namespace ServicesInterfaces.Companies
{
    //Interfaces for changing data in the database 
    public interface ICompanyCommand
    {
        Task UpdateCompanyAsync(CompanyDto company);
        Task DeleteCompanyAsync(int companyId);
        Task CreateCompanyAsync(CompanyDto company);
    }
}