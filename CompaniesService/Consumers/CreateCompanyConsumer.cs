using System.Threading.Tasks;
using MassTransit;
using ServicesInterfaces.Companies;
using TestBaseDto;

namespace CompaniesService.Consumers
{
    public class CreateCompanyConsumer : IConsumer<ICompanyCreate>
    {
        private readonly ICompanyService _companyService;

        public CreateCompanyConsumer(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        public async Task Consume(ConsumeContext<ICompanyCreate> context)
        {

            var companyDto = new CompanyDto
            {
                Name = context.Message.Name,
                Address = context.Message.Address,
                Phone = context.Message.Phone,
                CreatedDate = context.Message.CreatedDate,
                Users = context.Message.Users
            };

            await _companyService.CreateCompanyAsync(companyDto);

            await context.RespondAsync<ICompanyData>(new
            {
                context.Message.Name,
                context.Message.Address,
                context.Message.Phone,
                context.Message.CreatedDate,
            });
        }
    }
}
