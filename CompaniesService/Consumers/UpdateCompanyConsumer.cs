using System.Threading.Tasks;
using MassTransit;
using ServicesInterfaces.Companies;
using TestBaseDto;
using TestBaseDto.Company;

namespace CompaniesService.Consumers
{
    public class UpdateCompanyConsumer : IConsumer<ICompanyUpdate>
    {
        private readonly ICompanyCommand _companyCommand;

        public UpdateCompanyConsumer(ICompanyCommand companyCommand)
        {
            _companyCommand = companyCommand;
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

            await _companyCommand.UpdateCompanyAsync(company);

            await context.RespondAsync<ICompanyPrimaryData>(new {context.Message.Id});
        }
    }
}
