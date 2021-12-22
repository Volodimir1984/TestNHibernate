using System.Collections.Generic;
using TestBaseDto;

namespace ServicesInterfaces.Companies
{
    public interface ICompaniesData
    {
        IEnumerable<CompanyDto> Companies { get; set; }
    }
}
