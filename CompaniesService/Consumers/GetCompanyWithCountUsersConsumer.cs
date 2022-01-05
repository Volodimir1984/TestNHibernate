using System;
using System.Threading.Tasks;
using MassTransit;
using ServicesInterfaces.Companies;

namespace CompaniesService.Consumers
{
    public class GetCompanyWithCountUsersConsumer : IConsumer<ICountUsersInCompany>
    {
        private readonly ICompanyQuery _companyQ;

        public GetCompanyWithCountUsersConsumer(ICompanyQuery companyQ)
        {
            _companyQ = companyQ;
        }

        public async Task Consume(ConsumeContext<ICountUsersInCompany> context)
        {
            try
            {
                var companies = await _companyQ.GetCompanyWithCountUsersAsync(context.Message.CountUsers);

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
