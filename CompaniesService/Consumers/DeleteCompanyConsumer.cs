using System.Threading.Tasks;
using MassTransit;
using ServicesInterfaces.Companies;

namespace CompaniesService.Consumers
{
    public class DeleteCompanyConsumer : IConsumer<IDeleteCompany>
    {
        private readonly ICompanyService _companyService;

        public DeleteCompanyConsumer(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        public async Task Consume(ConsumeContext<IDeleteCompany> context)
        {
            await _companyService.DeleteCompanyAsync(context.Message.Id);

            await context.RespondAsync<ICompanyPrimaryData>(new {context.Message.Id});
        }
    }
}
