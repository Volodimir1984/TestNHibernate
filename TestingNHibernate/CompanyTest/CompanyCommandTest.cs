using AutoMapper;
using CompaniesService.Service;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using NUnit.Framework;
using ServicesInterfaces.Companies;
using System;
using System.Collections.Generic;
using System.Linq;
using TestBase;
using TestBase.Data;
using TestBaseDto.Company;
using TestBaseDto.Profeilers.Company;

namespace TestingNHibernate.CompanyTest
{
    public class CompanyCommandTest
    {
        private ICompanyCommand _companyCommand;

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

        private static IEnumerable<CompanyDto> _companiesDtoTest {

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

                yield return new CompanyDto
                {
                    Id = 2,
                    Name = "TestCompany5",
                    Address = "test Address",
                    CreatedDate = DateTime.Now.Date,
                    Phone = "80680024272"
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

            _companyCommand = new CompanyCommand(mockHibernateSession.Object, mapper);
        }

        [Test]
        [TestCaseSource("_companiesDtoTest")]
        public void TestUpdateCompanyAsync(CompanyDto companyDto)
        {
            Assert.DoesNotThrowAsync(() => _companyCommand.UpdateCompanyAsync(companyDto));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void TestDeleteCompanyAsync(int companyId)
        {
            Assert.DoesNotThrowAsync(() => _companyCommand.DeleteCompanyAsync(companyId));
        }

        [Test]
        [TestCase(3)]
        [TestCase(4)]
        public void TestDeleteCompanyAsyncException(int companyId)
        {
            Assert.ThrowsAsync<Exception>(() => _companyCommand.DeleteCompanyAsync(companyId));
        }

        [Test]
        [TestCaseSource("_companiesDtoTest")]
        public void TestCreateCompanyAsync(CompanyDto companyDto)
        {
            Assert.DoesNotThrowAsync(() => _companyCommand.CreateCompanyAsync(companyDto));
        }
    }

}
