using System.Threading.Tasks;
using MassTransit;
using ServicesInterfaces.Users;
using TestBaseDto;

namespace UsersService.Consumers
{
    public class UpdateUserConsumer : IConsumer<IUserUpdate>
    {
        private readonly IUserService _userService;

        public UpdateUserConsumer(IUserService userService)
        {
            _userService = userService;
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

            await _userService.UpdateUserAsync(user);

            await context.RespondAsync<IUserPrimaryData>(new {context.Message.Id});
        }
    }
}
