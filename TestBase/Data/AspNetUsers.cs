namespace TestBase.Data
{
    public class AspNetUsers
    {
        public virtual int Id { get;  set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Email { get; set; }
        public virtual Company Company { get; set; }
    }
}