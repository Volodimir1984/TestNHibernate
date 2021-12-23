using System.Threading.Tasks;
using MassTransit;
using ServicesInterfaces.Users;

namespace UsersService.Consumers
{
    public class GetUsersConsumer : IConsumer<IUsersData>
    {
        private readonly IUserService _userService;

        public GetUsersConsumer(IUserService userService)
        {
            _userService = userService;
        }

        public async Task Consume(ConsumeContext<IUsersData> context)
        {

            var users = await _userService.GetUsersAsync();

            await context.RespondAsync<IUsersData>(new
            {
                users
            });
        }
    }
}
