using System.Threading.Tasks;
using MassTransit;
using ServicesInterfaces.Companies;

namespace CompaniesService.Consumers
{
    public class GetCompanyConsumer : IConsumer<ICompanyPrimaryData>
    {
        private readonly ICompanyService _companyService;

        public GetCompanyConsumer(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        public async Task Consume(ConsumeContext<ICompanyPrimaryData> context)
        {

            var company = await _companyService.GetCompanyAsync(context.Message.Id);

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
