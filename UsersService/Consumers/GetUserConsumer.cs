using System.Threading.Tasks;
using MassTransit;
using ServicesInterfaces.Users;

namespace UsersService.Consumers
{
    public class GetUserConsumer : IConsumer<IUserPrimaryData>
    {
        private readonly IUserQuery _userQuery;

        public GetUserConsumer(IUserQuery userQuery)
        {
            _userQuery = userQuery;
        }

        public async Task Consume(ConsumeContext<IUserPrimaryData> context)
        {
            var user = await _userQuery.GetUserAsync(context.Message.Id);

            await context.RespondAsync<IUserData>(new
            {
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.CompanyId
            });
        }
    }
}
