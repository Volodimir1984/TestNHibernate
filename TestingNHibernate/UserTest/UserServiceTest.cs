using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using NUnit.Framework;
using ServicesInterfaces.Users;
using TestBase;
using TestBase.Data;
using TestBaseDto;
using TestBaseDto.Profeilers;
using UsersService.Service;

namespace TestingNHibernate.UserTest
{
    public class UserServiceTest
    {
        private IUserService _userService;

        private List<AspNetUsers> _listUsers = new List<AspNetUsers>
        {
            new AspNetUsers
            {
                Id = 1,
                FirstName = "Volodymyr",
                LastName = "Ivanciura",
                Email = "ibdwf@gmail.com",
                Company = new Company{Id = 1}
            },

            new AspNetUsers
            {
                Id = 2,
                FirstName = "Andriy",
                LastName = "Ivanciura",
                Email = "ibdeewwf@gmail.com",
                Company = new Company{Id = 2}
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

        private static IEnumerable<UserDto> _usersDtoTest
        {

            get
            {
                yield return new UserDto
                {
                    Id = 1,
                    FirstName = "Volodymyr",
                    LastName = "Ivanciura",
                    Email = "ibdwf@gmail.com",
                    CompanyId = 1
                };

                yield return new UserDto
                {
                    Id = 2,
                    FirstName = "Andriy",
                    LastName = "Ivanciura",
                    Email = "ibdeewwf@gmail.com",
                    CompanyId = 2
                };
            }
        }

        [SetUp]
        public void SetUp()
        {
            var mockHibernateSession = new Mock<INHibernateSession>();
            mockHibernateSession.Setup(s => s.Users)
                .Returns(new TestingQueryable<AspNetUsers>(_listUsers.AsQueryable()));


            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new UserDtoProfiler());
            });
            var mapper = config.CreateMapper();

            _userService = new UserService(mockHibernateSession.Object, mapper);
        }

        [Test]
        public async Task TestGetUsersAsync()
        {
            var companies = await _userService.GetUsersAsync();

            Assert.IsTrue(companies.Count() == 2);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public async Task TestGetUserAsync(int idCompany)
        {
            var company = await _userService.GetUserAsync(idCompany);

            Assert.IsTrue(company.Id == _listUsersDto[idCompany - 1].Id);
        }

        [Test]
        [TestCase(3)]
        [TestCase(4)]
        public void TestGetUserAsyncException(int companyId)
        {
            Assert.ThrowsAsync<UserServiceException>(() => _userService.GetUserAsync(companyId));
        }

        [Test]
        [TestCaseSource("_usersDtoTest")]
        public void TestUpdateUser(UserDto userDto)
        {
            Assert.DoesNotThrowAsync(() => _userService.UpdateUserAsync(userDto));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void TestDeleteUser(int userId)
        {
            Assert.DoesNotThrowAsync(() => _userService.DeleteUserAsync(userId));
        }

        [Test]
        [TestCase(3)]
        [TestCase(4)]
        public void TestDeleteUserException(int userId)
        {
            Assert.ThrowsAsync<Exception>(() => _userService.DeleteUserAsync(userId));
        }

        [Test]
        [TestCaseSource("_usersDtoTest")]
        public void TestCreateUser(UserDto userDto)
        {
            Assert.DoesNotThrowAsync(() => _userService.CreateUserAsync(userDto));
        }
    }
}
