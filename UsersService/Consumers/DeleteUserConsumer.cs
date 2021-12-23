using System.Threading.Tasks;
using MassTransit;
using ServicesInterfaces.Users;

namespace UsersService.Consumers
{
    public class DeleteUserConsumer : IConsumer<IUserDelete>
    {
        private readonly IUserService _userService;

        public DeleteUserConsumer(IUserService userService)
        {
            _userService = userService;
        }

        public async Task Consume(ConsumeContext<IUserDelete> context)
        {
            await _userService.DeleteUserAsync(context.Message.Id);

            await context.RespondAsync<IUserPrimaryData>(new {context.Message.Id});
        }
    }
}
