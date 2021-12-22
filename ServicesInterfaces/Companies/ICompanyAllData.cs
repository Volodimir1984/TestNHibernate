using System.Collections.Generic;
using TestBaseDto;

namespace ServicesInterfaces.Companies
{
    public interface ICompanyAllData : ICompanyData
    {
        IEnumerable<UserDto> Users { get; set; }
    }
}
