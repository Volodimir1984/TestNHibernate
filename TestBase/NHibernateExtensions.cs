using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using TestBase.Data;

namespace TestBase
{
    public static class NHibernateExtensions
    {
        //Extension method for turn on NHibernate which allow read and write data
        public static IServiceCollection AddNHibernate(this IServiceCollection serviceCollection,
            string connectionString)
        {

            var sessionFactory = CreateSessionFactory(connectionString);

            serviceCollection.AddSingleton(sessionFactory);
            serviceCollection.AddScoped(factory => sessionFactory.OpenSession());
            serviceCollection.AddScoped<INHibernateSession, NHibernateSession>();
            
            return serviceCollection;
        }

        //Extension method for turn on NHibernate which allow only read data
        public static IServiceCollection AddNHibernateQuery(this IServiceCollection serviceCollection,
            string connectionString)
        {

            var sessionFactory = CreateSessionFactory(connectionString);

            serviceCollection.AddSingleton(sessionFactory);
            serviceCollection.AddScoped(factory => sessionFactory.OpenSession());
            serviceCollection.AddScoped<INHibernateQuerySession, NHibernateQuerySession>();

            return serviceCollection;
        }

        private static ISessionFactory CreateSessionFactory(string connectionString)
        {
            var companyModel = AutoMap.AssemblyOf<Company>()
                .Where(t => t.Namespace == "TestBase.Data")
                .Override<Company>(map =>
                {
                    map.HasMany(x => x.Users).Cascade.All().Inverse();
                    map.Table("Companies");
                });

            var userModel = AutoMap.AssemblyOf<AspNetUsers>()
                .Where(t => t.Namespace == "TestBase.Data");

            return Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connectionString))
                .Mappings(m =>
                {
                    m.AutoMappings.Add(companyModel);
                    m.AutoMappings.Add(userModel);
                })
                .BuildSessionFactory();
        }
    }
}