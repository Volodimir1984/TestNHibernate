using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServicesInterfaces;
using TestBaseDto;


namespace TestNHibernate.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet("Companies")]
        public async Task<IActionResult> GetCompanies()
        {
            var companies = await _companyService.GetCompaniesAsync();
            return Ok(companies);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCompany(int id)
        {
            var company = await _companyService.GetCompanyAsync(id);
            return Ok(company);
        }

        [HttpPost("UpdateCompany")]
        public async Task<IActionResult> UpdateCompany(CompanyDto company)
        {
            await _companyService.UpdateCompanyAsync(company);
            return Ok();
        }

        [HttpDelete("DeleteCompany")]
        public async Task<IActionResult> DeleteCompany(CompanyDto company)
        {
            await _companyService.DeleteCompanyAsync(company.Id);
            return Ok();
        }

        [HttpPut("CreateCompany")]
        public async Task<IActionResult> CreateCompany(CompanyDto company)
        {
            await _companyService.CreateCompanyAsync(company);
            return Ok();
        }

        [HttpGet("GetCompanyWithCountUsers/{count:int}")]
        public async Task<IActionResult> GetCompanyWithCountUsers(int count)
        {
            var companies = await _companyService.GetCompanyWithCountUsersAsync(count);
            return Ok(companies);
        }
    }

}