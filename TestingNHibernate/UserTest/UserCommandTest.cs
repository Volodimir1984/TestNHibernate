using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using NUnit.Framework;
using ServicesInterfaces.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using TestBase;
using TestBase.Data;
using TestBaseDto.Profeilers.User;
using TestBaseDto.User;
using UsersService.Service;

namespace TestingNHibernate.UserTest
{
    public class UserCommandTest
    {
        private IUserCommand _userCommand;

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

            _userCommand = new UserCommand(mockHibernateSession.Object, mapper);
        }

        [Test]
        [TestCaseSource("_usersDtoTest")]
        public void TestUpdateUser(UserDto userDto)
        {
            Assert.DoesNotThrowAsync(() => _userCommand.UpdateUserAsync(userDto));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void TestDeleteUser(int userId)
        {
            Assert.DoesNotThrowAsync(() => _userCommand.DeleteUserAsync(userId));
        }

        [Test]
        [TestCase(3)]
        [TestCase(4)]
        public void TestDeleteUserException(int userId)
        {
            Assert.ThrowsAsync<Exception>(() => _userCommand.DeleteUserAsync(userId));
        }

        [Test]
        [TestCaseSource("_usersDtoTest")]
        public void TestCreateUser(UserDto userDto)
        {
            Assert.DoesNotThrowAsync(() => _userCommand.CreateUserAsync(userDto));
        }
    }
}
