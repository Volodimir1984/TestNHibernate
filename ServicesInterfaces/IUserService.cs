using System.Collections.Generic;
using System.Threading.Tasks;
using TestBaseDto;

namespace ServicesInterfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetUsersAsync();
        Task<UserDto> GetUserAsync(int id);
        Task UpdateUserAsync(UserDto user);
        Task DeleteUserAsync(int userId);
        Task CreateUserAsync(UserDto user);
    }
}