using System.Threading.Tasks;
using MassTransit;
using ServicesInterfaces.Users;

namespace UsersService.Consumers
{
    public class GetUsersConsumer : IConsumer<IUsersData>
    {
        private readonly IUserQuery _userQuery;

        public GetUsersConsumer(IUserQuery userQuery)
        {
            _userQuery = userQuery;
        }

        public async Task Consume(ConsumeContext<IUsersData> context)
        {

            var users = await _userQuery.GetUsersAsync();

            await context.RespondAsync<IUsersData>(new
            {
                users
            });
        }
    }
}
