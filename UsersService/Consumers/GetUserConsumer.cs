using System.Threading.Tasks;
using MassTransit;
using ServicesInterfaces.Users;

namespace UsersService.Consumers
{
    public class GetUserConsumer : IConsumer<IUserPrimaryData>
    {
        private readonly IUserService _userService;

        public GetUserConsumer(IUserService userService)
        {
            _userService = userService;
        }

        public async Task Consume(ConsumeContext<IUserPrimaryData> context)
        {
            var user = await _userService.GetUserAsync(context.Message.Id);

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
