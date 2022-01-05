using System.Linq;
using TestBase.Data;

namespace TestBase
{
    public interface INHibernateQuerySession
    {
        IQueryable<Company> Companies { get; }
        IQueryable<AspNetUsers> Users { get; }
    }
}
