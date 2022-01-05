using MassTransit;
using ServicesInterfaces.Users;
using System.Threading.Tasks;
using TestBaseDto.User;

namespace UsersService.Consumers
{
    public class UpdateUserConsumer : IConsumer<IUserUpdate>
    {
        private readonly IUserCommand _userCommand;

        public UpdateUserConsumer(IUserCommand userCommand)
        {
            _userCommand = userCommand;
        }

        public async Task Consume(ConsumeContext<IUserUpdate> context)
        {
            var user = new UserDto
            {
                Id = context.Message.Id,
                FirstName = context.Message.FirstName,
                LastName = context.Message.LastName,
                Email = context.Message.Email,
                CompanyId = context.Message.CompanyId,
            };

            await _userCommand.UpdateUserAsync(user);

            await context.RespondAsync<IUserPrimaryData>(new {context.Message.Id});
        }
    }
}
