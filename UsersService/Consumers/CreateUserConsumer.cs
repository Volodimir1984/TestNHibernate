using MassTransit;
using ServicesInterfaces.Users;
using System.Threading.Tasks;
using TestBaseDto.User;

namespace UsersService.Consumers
{
    public class CreateUserConsumer : IConsumer<IUserCreate>
    {
        private readonly IUserCommand _userCommand;

        public CreateUserConsumer(IUserCommand userCommand)
        {
            _userCommand = userCommand;
        }

        public async Task Consume(ConsumeContext<IUserCreate> context)
        {
            var user = new UserDto
            {
                Id = context.Message.Id,
                FirstName = context.Message.FirstName,
                LastName = context.Message.LastName,
                Email = context.Message.Email,
                CompanyId = context.Message.CompanyId,
            };

            await _userCommand.CreateUserAsync(user);

            await context.RespondAsync<IUserData>(new
            {
                context.Message.Id,
                context.Message.FirstName,
                context.Message.LastName,
                context.Message.Email,
                context.Message.CompanyId,
            });
        }
    }
}
