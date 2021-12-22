namespace ServicesInterfaces.Users
{
    public interface IUserData : IUserPrimaryData
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
        int CompanyId { get; set; }
    }
}
