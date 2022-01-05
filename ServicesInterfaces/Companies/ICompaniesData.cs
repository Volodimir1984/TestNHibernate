using System.Collections.Generic;
using TestBaseDto;
using TestBaseDto.Company;

namespace ServicesInterfaces.Companies
{
    public interface ICompaniesData
    {
        IEnumerable<CompanyAllDataDto> Companies { get; set; }
    }
}
