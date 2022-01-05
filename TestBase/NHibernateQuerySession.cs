using System.Collections.Generic;
using System.Linq;
using NHibernate;
using TestBase.Data;

namespace TestBase
{
    public class NHibernateQuerySession: INHibernateQuerySession
    {
        private readonly ISession _session;

        public NHibernateQuerySession(IEnumerable<ISession> sessions)
        {
            _session = sessions.Last();
        }

        public IQueryable<Company> Companies => _session.Query<Company>();
        public IQueryable<AspNetUsers> Users => _session.Query<AspNetUsers>();
    }
}
