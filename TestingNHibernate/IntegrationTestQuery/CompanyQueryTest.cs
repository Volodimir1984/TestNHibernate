using AutoMapper;
using CompaniesService.Service;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NUnit.Framework;
using ServicesInterfaces.Companies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using TestBase;
using TestBase.Data;
using TestBaseDto.Company;
using TestBaseDto.Profeilers.Company;

namespace TestingNHibernate.IntegrationTestQuery
{
    public class CompanyQueryTest
    {
        private ISession _commandSession;
        private ISession _querySession;

        private ICompanyCommand _companyCommand;
        private ICompanyCommand _companyWithQuery;
        private ICompanyQuery _companyQuery;

        private const string _commandStringConnection =
            "Data Source=(local);Integrated Security=True;Initial Catalog=ppim-dev;User ID=sa;password=Volodymyr@10091984";

        private const string _queryStringConnection =
            "Data Source=(local);Initial Catalog=ppim-dev;User ID=Volodymyr3;password=Volodymyr10091984";

        private static IEnumerable<CompanyDto> _companiesDtoTest
        {

            get
            {
                yield return new CompanyDto
                {
                    Id = 1,
                    Name = "TestCompany4",
                    Address = "test Address",
                    CreatedDate = DateTime.Now.Date,
                    Phone = "80680024272"
                };
            }
        }

        private ISessionFactory CreateSessionFactory(string connectionString)
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

        [SetUp]
        public void SetUp()
        {
            var commandFactory = CreateSessionFactory(_commandStringConnection);
            _commandSession = commandFactory.OpenSession();

            var queryFactory = CreateSessionFactory(_queryStringConnection);
            _querySession = queryFactory.OpenSession();

            var sessions = new List<ISession> {_commandSession, _querySession};
            var sessionsQuery = new List<ISession> { _querySession};

            var nHibernateSession = new NHibernateSession(sessions);
            var nHibernateWithQuerySession = new NHibernateSession(sessionsQuery);
            var nHibernateQuerySession = new NHibernateQuerySession(sessions);

            var configWrite = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new CompanyDtoProfiler());
            });
            var mapperWrite = configWrite.CreateMapper();

            var configRead = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new CompanyAllDataDtoProfiler());
            });
            var mapperRead = configRead.CreateMapper();

            var cache = new Mock<IDistributedCache>();

            _companyCommand = new CompanyCommand(nHibernateSession, mapperWrite);
            _companyWithQuery = new CompanyCommand(nHibernateWithQuerySession, mapperWrite);
            _companyQuery = new CompanyQuery(nHibernateQuerySession, mapperRead, cache.Object);
        }

        [Test]
        [TestCaseSource("_companiesDtoTest")]
        public void TestUpdateCompanyAsync(CompanyDto companyDto)
        {
            Assert.DoesNotThrowAsync(() => _companyCommand.UpdateCompanyAsync(companyDto));
        }

        [Test]
        [TestCaseSource("_companiesDtoTest")]
        public void TestUpdateCompanyExceptionAsync(CompanyDto companyDto)
        {
            Assert.ThrowsAsync<Exception>(() => _companyWithQuery.UpdateCompanyAsync(companyDto));
        }

        [Test]
        public async Task TestGetCompaniesAsync()
        {
            var companies = await _companyQuery.GetCompaniesAsync();

            Assert.IsTrue(companies.Count() == 72);
        }
    }
}
