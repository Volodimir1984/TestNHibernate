using System.Collections.Generic;
using TestBaseDto;

namespace ServicesInterfaces.Users
{
    public interface IUsersData
    {
        IEnumerable<UserDto> Users { get; set; }
    }
}
