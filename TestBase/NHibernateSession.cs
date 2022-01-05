using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NHibernate;
using TestBase.Data;

namespace TestBase
{
    public class NHibernateSession: INHibernateSession
    {
        private readonly ISession _session;
        private ITransaction _transaction;

        public NHibernateSession(IEnumerable<ISession> sessions)
        {
            _session = sessions.First();
        }

        public void BeginTransaction()
        {
            _transaction = _session.BeginTransaction();
        }

        public async Task CommitAsync()
        {
            await _transaction.CommitAsync();
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
                await _transaction.RollbackAsync();
        }

        public void CloseTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }
        
        public async Task SaveAsync<T>(T company)
        {
            await _session.SaveOrUpdateAsync(company);
        }

        public async Task DeleteAsync<T>(T company)
        {
            await _session.DeleteAsync(company);
        }

        public IQueryable<Company> Companies => _session.Query<Company>();
        public IQueryable<AspNetUsers> Users => _session.Query<AspNetUsers>();
    }
}