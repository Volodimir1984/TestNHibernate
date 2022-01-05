namespace TestBaseDto.User
{
    public class UserDto: UserPrimaryDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int CompanyId { get; set; }
    }
}