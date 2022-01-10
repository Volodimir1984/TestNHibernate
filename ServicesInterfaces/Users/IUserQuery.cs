using System.Collections.Generic;
using System.Threading.Tasks;
using TestBaseDto.User;

namespace ServicesInterfaces.Users
{
    //Interfaces for read data from database
    public interface IUserQuery
    {
        Task<IEnumerable<UserDto>> GetUsersAsync();
        Task<UserDto> GetUserAsync(int id);
    }
}
