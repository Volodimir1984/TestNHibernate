using System;
using System.Collections.Generic;

namespace TestBaseDto
{
    public class CompanyDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedDate { get; set; }
        public IEnumerable<UserDto> Users { get; set; }
    }
}