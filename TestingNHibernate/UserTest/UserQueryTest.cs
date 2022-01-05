using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using NUnit.Framework;
using ServicesInterfaces.Users;
using TestBase;
using TestBase.Data;
using TestBaseDto.Profeilers.User;
using TestBaseDto.User;
using UsersService.Service;

namespace TestingNHibernate.UserTest
{
    public class UserQueryTest
    {
        private IUserQuery _userQuery;

        private List<AspNetUsers> _listUsers = new List<AspNetUsers>
        {
            new AspNetUsers
            {
                Id = 1,
                FirstName = "Volodymyr",
                LastName = "Ivanciura",
                Email = "ibdwf@gmail.com",
                Company = new Company {Id = 1}
            },

            new AspNetUsers
            {
                Id = 2,
                FirstName = "Andriy",
                LastName = "Ivanciura",
                Email = "ibdeewwf@gmail.com",
                Company = new Company {Id = 2}
            },
        };

        private List<UserDto> _listUsersDto = new List<UserDto>
        {

            new UserDto
            {
                Id = 1,
                FirstName = "Volodymyr",
                LastName = "Ivanciura",
                Email = "ibdwf@gmail.com",
                CompanyId = 1
            },

            new UserDto
            {
                Id = 2,
                FirstName = "Andriy",
                LastName = "Ivanciura",
                Email = "ibdeewwf@gmail.com",
                CompanyId = 2
            },
        };

        [SetUp]
        public void SetUp()
        {
            var mockHibernateSession = new Mock<INHibernateQuerySession>();
            mockHibernateSession.Setup(s => s.Users)
                .Returns(new TestingQueryable<AspNetUsers>(_listUsers.AsQueryable()));


            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new UserDtoProfiler());
            });
            var mapper = config.CreateMapper();

            var cache = new Mock<IDistributedCache>();

            _userQuery = new UserQuery(mockHibernateSession.Object, mapper, cache.Object);
        }

        [Test]
        public async Task TestGetUsersAsync()
        {
            var companies = await _userQuery.GetUsersAsync();

            Assert.IsTrue(companies.Count() == 2);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public async Task TestGetUserAsync(int idCompany)
        {
            var company = await _userQuery.GetUserAsync(idCompany);

            Assert.IsTrue(company.Id == _listUsersDto[idCompany - 1].Id);
        }

        [Test]
        [TestCase(3)]
        [TestCase(4)]
        public void TestGetUserAsyncException(int companyId)
        {
            Assert.ThrowsAsync<UserServiceException>(() => _userQuery.GetUserAsync(companyId));
        }
    }
}
