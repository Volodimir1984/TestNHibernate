using System.Linq;
using System.Threading.Tasks;
using TestBase.Data;

namespace TestBase
{
    public interface INHibernateSession
    {
        void BeginTransaction();
        Task CommitAsync();
        Task RollbackAsync();
        void CloseTransaction();
        Task SaveAsync<T>(T data);
        Task DeleteAsync<T>(T id);
        
        IQueryable<Company> Companies { get; }
        IQueryable<AspNetUsers> Users { get; }
    }
}