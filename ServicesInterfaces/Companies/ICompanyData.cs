using System;

namespace ServicesInterfaces.Companies
{
    public interface ICompanyData : ICompanyPrimaryData
    {
        string Name { get; set; }
        string Address { get; set; }
        string Phone { get; set; }
        DateTime CreatedDate { get; set; }
    }
}
