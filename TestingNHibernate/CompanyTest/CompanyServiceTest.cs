using AutoMapper;
using Moq;
using NUnit.Framework;
using ServicesInterfaces.Companies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompaniesService.Service;
using Microsoft.Extensions.Caching.Distributed;
using TestBase;
using TestBase.Data;
using TestBaseDto;
using TestBaseDto.Profeilers;

namespace TestingNHibernate.CompanyTest
{
    public class CompanyServiceTest
    {
        private ICompanyService _companyService;

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

        private List<CompanyDto> _listCompaniesDto = new List<CompanyDto>
        {
            new CompanyDto
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

            new CompanyDto
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

        private static IEnumerable<CompanyDto> _companiesDtoTest {

            get
            {
                yield return new CompanyDto
                {
                    Id = 1,
                    Name = "TestCompany4",
                    Address = "test Address",
                    CreatedDate = DateTime.Now.Date,
                    Phone = "80680024272",
                    Users = new List<UserDto>
                    {
                        new UserDto {Id = 1, FirstName = "Andriy", LastName = "Ivanciura", CompanyId = 2}
                    }
                };

                yield return new CompanyDto
                {
                    Id = 2,
                    Name = "TestCompany5",
                    Address = "test Address",
                    CreatedDate = DateTime.Now.Date,
                    Phone = "80680024272",
                    Users = new List<UserDto>
                    {
                        new UserDto {Id = 2, FirstName = "Andriy", LastName = "Ivanciura", CompanyId = 2}
                    }
                };
            }
        }

        [SetUp]
        public void SetUp()
        { 
            var mockHibernateSession = new Mock<INHibernateSession>();
            mockHibernateSession.Setup(s => s.Companies)
                .Returns(new TestingQueryable<Company>(_listCompanies.AsQueryable()));


            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new CompanyDtoProfiler());
            });
            var mapper = config.CreateMapper();

            var cache = new Mock<IDistributedCache>();

            _companyService = new CompanyService(mockHibernateSession.Object, mapper, cache.Object);
        }

        [Test]
        public async Task TestGetCompaniesAsync()
        {
            var companies = await _companyService.GetCompaniesAsync();

            Assert.IsTrue(companies.Count() == 2);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public async Task TestGetCompanyAsync(int idCompany)
        {
            var company = await _companyService.GetCompanyAsync(idCompany);

            Assert.IsTrue(company.Id == _listCompaniesDto[idCompany - 1].Id);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public async Task TestGetCompanyWithCountUsersAsync(int count)
        {
            var companies = await _companyService.GetCompanyWithCountUsersAsync(count);
            Assert.IsTrue(_listCompanies.Count(c => c.Users.Count >= count) == companies.Count());
        }

        [Test]
        [TestCase(3)]
        [TestCase(4)]
        public void TestGetCompanyAsyncException(int companyId)
        {
            Assert.ThrowsAsync<CompanyServiceException>(() =>_companyService.GetCompanyAsync(companyId));
        }

        [Test]
        [TestCaseSource("_companiesDtoTest")]
        public void TestUpdateCompanyAsync(CompanyDto companyDto)
        {
            Assert.DoesNotThrowAsync(() => _companyService.UpdateCompanyAsync(companyDto));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void TestDeleteCompanyAsync(int companyId)
        {
            Assert.DoesNotThrowAsync(() => _companyService.DeleteCompanyAsync(companyId));
        }

        [Test]
        [TestCase(3)]
        [TestCase(4)]
        public void TestDeleteCompanyAsyncException(int companyId)
        {
            Assert.ThrowsAsync<Exception>(() => _companyService.DeleteCompanyAsync(companyId));
        }

        [Test]
        [TestCaseSource("_companiesDtoTest")]
        public void TestCreateCompanyAsync(CompanyDto companyDto)
        {
            Assert.DoesNotThrowAsync(() => _companyService.CreateCompanyAsync(companyDto));
        }
    }

}
