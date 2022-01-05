using System.Collections.Generic;
using System.Threading.Tasks;
using TestBaseDto;
using TestBaseDto.User;

namespace ServicesInterfaces.Users
{
    public interface IUserCommand
    {
        Task UpdateUserAsync(UserDto user);
        Task DeleteUserAsync(int userId);
        Task CreateUserAsync(UserDto user);
    }
}