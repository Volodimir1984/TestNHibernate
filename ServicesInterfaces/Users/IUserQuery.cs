using System.Collections.Generic;
using System.Threading.Tasks;
using TestBaseDto;
using TestBaseDto.User;

namespace ServicesInterfaces.Users
{
    public interface IUserQuery
    {
        Task<IEnumerable<UserDto>> GetUsersAsync();
        Task<UserDto> GetUserAsync(int id);
    }
}
