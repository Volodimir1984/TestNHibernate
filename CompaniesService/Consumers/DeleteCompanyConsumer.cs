using System.Threading.Tasks;
using MassTransit;
using ServicesInterfaces.Companies;

namespace CompaniesService.Consumers
{
    public class DeleteCompanyConsumer : IConsumer<IDeleteCompany>
    {
        private readonly ICompanyCommand _companyCommand;

        public DeleteCompanyConsumer(ICompanyCommand companyCommand)
        {
            _companyCommand = companyCommand;
        }

        public async Task Consume(ConsumeContext<IDeleteCompany> context)
        {
            await _companyCommand.DeleteCompanyAsync(context.Message.Id);

            await context.RespondAsync<ICompanyPrimaryData>(new {context.Message.Id});
        }
    }
}
