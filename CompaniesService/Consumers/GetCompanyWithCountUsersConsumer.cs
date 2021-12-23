using System;
using System.Threading.Tasks;
using MassTransit;
using ServicesInterfaces.Companies;

namespace CompaniesService.Consumers
{
    public class GetCompanyWithCountUsersConsumer : IConsumer<ICountUsersInCompany>
    {
        private readonly ICompanyService _companyService;

        public GetCompanyWithCountUsersConsumer(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        public async Task Consume(ConsumeContext<ICountUsersInCompany> context)
        {
            try
            {
                var companies = await _companyService.GetCompanyWithCountUsersAsync(context.Message.CountUsers);

                await context.RespondAsync<ICountUsersInCompany>(new
                {
                    companies,
                    context.Message.CountUsers
                });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
