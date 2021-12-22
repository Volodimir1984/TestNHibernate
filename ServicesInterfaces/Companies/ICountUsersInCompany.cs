namespace ServicesInterfaces.Companies
{
    public interface ICountUsersInCompany : ICompaniesData
    {
        int CountUsers { get; set; }
    }
}
