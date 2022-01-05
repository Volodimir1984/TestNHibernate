using MassTransit;
using ServicesInterfaces.Companies;
using System.Threading.Tasks;
using TestBaseDto.Company;

namespace CompaniesService.Consumers
{
    public class CreateCompanyConsumer : IConsumer<ICompanyCreate>
    {
        private readonly ICompanyCommand _companyCommand;

        public CreateCompanyConsumer(ICompanyCommand companyCommand)
        {
            _companyCommand = companyCommand;
        }

        public async Task Consume(ConsumeContext<ICompanyCreate> context)
        {

            var companyDto = new CompanyDto
            {
                Name = context.Message.Name,
                Address = context.Message.Address,
                Phone = context.Message.Phone,
                CreatedDate = context.Message.CreatedDate
            };

            await _companyCommand.CreateCompanyAsync(companyDto);

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
