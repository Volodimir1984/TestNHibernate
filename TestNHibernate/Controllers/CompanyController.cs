using MassTransit;
using Microsoft.AspNetCore.Mvc;
using ServicesInterfaces.Companies;
using System.Threading.Tasks;
using TestBaseDto;
using TestBaseDto.Company;


namespace TestNHibernate.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly IRequestClient<ICompanyPrimaryData> _companyRequestClient;
        private readonly IRequestClient<ICompaniesData> _companiesRequestClient;
        private readonly IRequestClient<ICountUsersInCompany> _countUsersRequestClient;
        private readonly IRequestClient<ICompanyUpdate> _updateCompanyRequestClient;
        private readonly IRequestClient<ICompanyCreate> _createCompanyRequestClient;
        private readonly IRequestClient<IDeleteCompany> _deleteCompanyRequestClient;

        public CompanyController(IRequestClient<ICompanyPrimaryData> companyRequestClient, 
            IRequestClient<ICompaniesData> companiesRequestClient,
            IRequestClient<ICountUsersInCompany> countUsersRequestClient, 
            IRequestClient<ICompanyUpdate> updateCompanyRequestClient, IRequestClient<ICompanyCreate> createCompanyRequestClient, IRequestClient<IDeleteCompany> deleteCompanyRequestClient)
        {
            _companyRequestClient = companyRequestClient;
            _companiesRequestClient = companiesRequestClient;
            _countUsersRequestClient = countUsersRequestClient;
            _updateCompanyRequestClient = updateCompanyRequestClient;
            _createCompanyRequestClient = createCompanyRequestClient;
            _deleteCompanyRequestClient = deleteCompanyRequestClient;
        }

        [HttpGet("Companies")]
        public async Task<IActionResult> GetCompanies()
        {
            var companies = await _companiesRequestClient.GetResponse<ICompaniesData>(new {});
            return Ok(companies.Message);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCompany(int id)
        {
            var company = await _companyRequestClient.GetResponse<ICompanyAllData>(new
            {
                Id = id
            });

            return Ok(company.Message);
        }

        [HttpPost("UpdateCompany")]
        public async Task<IActionResult> UpdateCompany(CompanyDto company)
        {
            var companyId = await _updateCompanyRequestClient.GetResponse<ICompanyPrimaryData>(new
            {
                company.Id,
                company.Name,
                company.Address,
                company.Phone,
                company.CreatedDate
            });

            return Ok(companyId.Message);
        }

        [HttpDelete("DeleteCompany")]
        public async Task<IActionResult> DeleteCompany(CompanyPrimaryDto company)
        {
            var companyId = await _deleteCompanyRequestClient.GetResponse<ICompanyPrimaryData>(new {company.Id});
  
            return Ok(companyId.Message);
        }

        [HttpPut("CreateCompany")]
        public async Task<IActionResult> CreateCompany(CompanyDto company)
        {
            var companyData= await _createCompanyRequestClient.GetResponse<ICompanyData>(new
            {
                company.Name,
                company.Address,
                company.Phone,
                company.CreatedDate
            });

            return Ok(companyData.Message);
        }

        [HttpGet("GetCompanyWithCountUsers/{count:int}")]
        public async Task<IActionResult> GetCompanyWithCountUsers(int count)
        {
            var companies = await _countUsersRequestClient.GetResponse<ICountUsersInCompany>(new
            {
                CountUsers = count
            });

            return Ok(companies.Message);
        }
    }

}