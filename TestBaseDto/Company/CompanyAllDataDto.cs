using System.Collections.Generic;
using TestBaseDto.User;

namespace TestBaseDto.Company
{
    public class CompanyAllDataDto : CompanyDto
    {
        public IEnumerable<UserDto> Users { get; set; }
    }
}
