using System.Threading.Tasks;
using MassTransit;
using ServicesInterfaces.Companies;

namespace CompaniesService.Consumers
{
    public class GetCompanyConsumer : IConsumer<ICompanyPrimaryData>
    {
        private readonly ICompanyQuery _companyQuery;

        public GetCompanyConsumer(ICompanyQuery companyQuery)
        {
            _companyQuery = companyQuery;
        }

        public async Task Consume(ConsumeContext<ICompanyPrimaryData> context)
        {

            var company = await _companyQuery.GetCompanyAsync(context.Message.Id);

            await context.RespondAsync<ICompanyAllData>(new
            {
                company.Id,
                company.Name,
                company.Address,
                company.Phone,
                company.CreatedDate,
                company.Users
            });
        }
    }
}
