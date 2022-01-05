using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CompaniesService.Service;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using NUnit.Framework;
using ServicesInterfaces.Companies;
using TestBase;
using TestBase.Data;
using TestBaseDto.Company;
using TestBaseDto.Profeilers.Company;
using TestBaseDto.User;

namespace TestingNHibernate.CompanyTest
{
    public class CompanyQueryTest
    {
        private ICompanyQuery _companyQuery;

        private List<Company> _listCompanies = new List<Company>
        {
            new Company
            {
                Id = 1,
                Name = "TestCompany",
                Adress = "test Address",
                CreatedDate = DateTime.Now.Date,
                Phone = "80680024272",
                Users = new List<AspNetUsers>
                {
                    new AspNetUsers {Id = 1, FirstName = "Volodymyr", LastName = "Ivanciura"}
                }
            },

            new Company
            {
                Id = 2,
                Name = "TestCompany1",
                Adress = "test Address1",
                CreatedDate = DateTime.Now.Date,
                Phone = "80680024273",
                Users = new List<AspNetUsers>
                {
                    new AspNetUsers {Id = 2, FirstName = "Andriy", LastName = "Ivanciura"},
                    new AspNetUsers {Id = 3, FirstName = "Andriy", LastName = "Ivanciura"}
                }
            },
        };

        private List<CompanyAllDataDto> _listCompaniesDto = new List<CompanyAllDataDto>
        {
            new CompanyAllDataDto
            {
                Id = 1,
                Name = "TestCompany",
                Address = "test Address",
                CreatedDate = DateTime.Now.Date,
                Phone = "80680024272",
                Users = new List<UserDto>
                {
                    new UserDto
                    {
                        Id = 1, FirstName = "Volodymyr", LastName = "Ivanciura", CompanyId = 1
                    }
                }
            },

            new CompanyAllDataDto
            {
                Id = 2,
                Name = "TestCompany1",
                Address = "test Address",
                CreatedDate = DateTime.Now.Date,
                Phone = "80680024273",
                Users = new List<UserDto>
                {
                    new UserDto {Id = 2, FirstName = "Andriy", LastName = "Ivanciura", CompanyId = 2},
                }
            },
        };

        [SetUp]
        public void SetUp()
        {
            var mockHibernateSession = new Mock<INHibernateQuerySession>();
            mockHibernateSession.Setup(s => s.Companies)
                .Returns(new TestingQueryable<Company>(_listCompanies.AsQueryable()));


            var config = new MapperConfiguration(cfg => { cfg.AddProfile(new CompanyAllDataDtoProfiler()); });
            var mapper = config.CreateMapper();

            var cache = new Mock<IDistributedCache>();

            _companyQuery = new CompanyQuery(mockHibernateSession.Object, mapper, cache.Object);
        }

        [Test]
        public async Task TestGetCompaniesAsync()
        {
            var companies = await _companyQuery.GetCompaniesAsync();

            Assert.IsTrue(companies.Count() == 2);
        }

        [Test]
        [TestCase(3)]
        [TestCase(4)]
        public void TestGetCompanyAsyncException(int companyId)
        {
            Assert.ThrowsAsync<CompanyServiceException>(() => _companyQuery.GetCompanyAsync(companyId));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public async Task TestGetCompanyAsync(int idCompany)
        {
            var company = await _companyQuery.GetCompanyAsync(idCompany);

            Assert.IsTrue(company.Id == _listCompaniesDto[idCompany - 1].Id);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public async Task TestGetCompanyWithCountUsersAsync(int count)
        {
            var companies = await _companyQuery.GetCompanyWithCountUsersAsync(count);
            Assert.IsTrue(_listCompanies.Count(c => c.Users.Count >= count) == companies.Count());
        }
    }
}
