using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.DependencyInjection;
using TestBase.Data;

namespace TestBase
{
    public static class NHibernateExtensions
    {
        public static IServiceCollection AddNHibernate(this IServiceCollection serviceCollection,
            string connectionString)
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

            var sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connectionString))
                .Mappings(m =>
                {
                    m.AutoMappings.Add(companyModel);
                    m.AutoMappings.Add(userModel);
                })
                .BuildSessionFactory();

            serviceCollection.AddSingleton(sessionFactory); 
            serviceCollection.AddScoped(factory => sessionFactory.OpenSession());
            serviceCollection.AddScoped<INHibernateSession, NHibernateSession>();
            
            return serviceCollection;
        }
    }
}