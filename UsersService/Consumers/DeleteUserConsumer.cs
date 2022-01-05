using System.Threading.Tasks;
using MassTransit;
using ServicesInterfaces.Users;

namespace UsersService.Consumers
{
    public class DeleteUserConsumer : IConsumer<IUserDelete>
    {
        private readonly IUserCommand _userCommand;

        public DeleteUserConsumer(IUserCommand userCommand)
        {
            _userCommand = userCommand;
        }

        public async Task Consume(ConsumeContext<IUserDelete> context)
        {
            await _userCommand.DeleteUserAsync(context.Message.Id);

            await context.RespondAsync<IUserPrimaryData>(new {context.Message.Id});
        }
    }
}
