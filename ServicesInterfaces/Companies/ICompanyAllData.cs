using System.Collections.Generic;
using TestBaseDto;
using TestBaseDto.User;

namespace ServicesInterfaces.Companies
{
    public interface ICompanyAllData : ICompanyData
    {
        IEnumerable<UserDto> Users { get; set; }
    }
}
