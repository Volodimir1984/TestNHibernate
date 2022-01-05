using System;

namespace TestBaseDto.Company
{
    public class CompanyDto: CompanyPrimaryDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}