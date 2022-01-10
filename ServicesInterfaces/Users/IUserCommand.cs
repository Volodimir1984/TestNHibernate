using System.Threading.Tasks;
using TestBaseDto.User;

namespace ServicesInterfaces.Users
{
    //Interfaces for changing data in the database 
    public interface IUserCommand
    {
        Task UpdateUserAsync(UserDto user);
        Task DeleteUserAsync(int userId);
        Task CreateUserAsync(UserDto user);
    }
}