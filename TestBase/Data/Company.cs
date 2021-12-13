using System;
using System.Collections.Generic;

namespace TestBase.Data
{
    public class Company
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Adress { get; set; }
        public virtual string Phone { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual IList<AspNetUsers> Users { get; set; }
    }
}