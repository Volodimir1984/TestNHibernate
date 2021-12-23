using System;
using System.Threading.Tasks;
using MassTransit;
using ServicesInterfaces.Companies;

namespace CompaniesService.Consumers
{
    public class GetCompaniesConsumer : IConsumer<ICompaniesData>
    {
        private readonly ICompanyService _companyService;

        public GetCompaniesConsumer(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        public async Task Consume(ConsumeContext<ICompaniesData> context)
        {
            try
            {
                var companies = await _companyService.GetCompaniesAsync();

                await context.RespondAsync<ICompaniesData>(new
                {
                    Companies = companies
                });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
