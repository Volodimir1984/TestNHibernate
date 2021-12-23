using System.Threading.Tasks;
using MassTransit;
using ServicesInterfaces.Companies;
using TestBaseDto;

namespace CompaniesService.Consumers
{
    public class UpdateCompanyConsumer : IConsumer<ICompanyUpdate>
    {
        private readonly ICompanyService _companyService;

        public UpdateCompanyConsumer(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        public async Task Consume(ConsumeContext<ICompanyUpdate> context)
        {
            var company = new CompanyDto
            {
                Id = context.Message.Id,
                Name = context.Message.Name,
                Address = context.Message.Address,
                Phone = context.Message.Phone,
                CreatedDate = context.Message.CreatedDate
            };

            await _companyService.UpdateCompanyAsync(company);

            await context.RespondAsync<ICompanyPrimaryData>(new {context.Message.Id});
        }
    }
}
