using System.Threading.Tasks;
using MassTransit;
using ServicesInterfaces.Users;
using TestBaseDto;

namespace UsersService.Consumers
{
    public class CreateUserConsumer : IConsumer<IUserCreate>
    {
        private readonly IUserService _userService;

        public CreateUserConsumer(IUserService userService)
        {
            _userService = userService;
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

            await _userService.CreateUserAsync(user);

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
