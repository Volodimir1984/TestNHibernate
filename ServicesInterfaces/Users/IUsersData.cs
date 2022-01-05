using System.Collections.Generic;
using TestBaseDto;
using TestBaseDto.User;

namespace ServicesInterfaces.Users
{
    public interface IUsersData
    {
        IEnumerable<UserDto> Users { get; set; }
    }
}
