using ConstData;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using ServicesInterfaces.Companies;
using System;
using System.Threading.Tasks;
using TestBaseDto;


namespace TestNHibernate.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly IRequestClient<ICompanyPrimaryData> _companyRequestClient;
        private readonly IRequestClient<ICompaniesData> _companiesRequestClient;
        private readonly IRequestClient<ICountUsersInCompany> _countUsersRequestClient;
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public CompanyController(IRequestClient<ICompanyPrimaryData> companyRequestClient, 
            IRequestClient<ICompaniesData> companiesRequestClient,
            ISendEndpointProvider sendEndpointProvider, 
            IRequestClient<ICountUsersInCompany> countUsersRequestClient)
        {
            _companyRequestClient = companyRequestClient;
            _companiesRequestClient = companiesRequestClient;
            _sendEndpointProvider = sendEndpointProvider;
            _countUsersRequestClient = countUsersRequestClient;
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
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{ConstStringForQueues.UpdateCompany}"));
            await endpoint.Send<ICompanyData>(new
            {
                company.Id,
                company.Name,
                company.Address,
                company.Phone,
                company.CreatedDate
            });

            return Ok();
        }

        [HttpDelete("DeleteCompany")]
        public async Task<IActionResult> DeleteCompany(CompanyDto company)
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{ConstStringForQueues.DeleteCompany}"));
            await endpoint.Send<ICompanyPrimaryData>(new
            {
                company.Id
            });

            return Ok();
        }

        [HttpPut("CreateCompany")]
        public async Task<IActionResult> CreateCompany(CompanyDto company)
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{ConstStringForQueues.CreateCompany}"));
            await endpoint.Send<ICompanyAllData>(new
            {
                company.Name,
                company.Address,
                company.Phone,
                company.CreatedDate,
                company.Users
            });

            return Ok();
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