using System;
using System.Threading.Tasks;
using MassTransit;
using ServicesInterfaces.Companies;

namespace CompaniesService.Consumers
{
    public class GetCompaniesConsumer : IConsumer<ICompaniesData>
    {
        private readonly ICompanyQuery _companyQuery;

        public GetCompaniesConsumer(ICompanyQuery companyQuery)
        {
            _companyQuery = companyQuery;
        }

        public async Task Consume(ConsumeContext<ICompaniesData> context)
        {
            try
            {
                var companies = await _companyQuery.GetCompaniesAsync();

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
