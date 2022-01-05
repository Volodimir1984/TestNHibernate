using AutoMapper;
using System.Linq;
using TestBaseDto.Company;
using TestBaseDto.User;

namespace TestBaseDto.Profeilers.Company
{
    public class CompanyAllDataDtoProfiler: Profile
    {
        public CompanyAllDataDtoProfiler()
        {
            CreateMap<TestBase.Data.Company, CompanyAllDataDto>()
                .ForMember(dto => dto.Address, entity => entity.MapFrom(c => c.Adress))
                .ForMember(dto => dto.Users, entity => entity.MapFrom(c => c.Users
                    .Select(u => new UserDto
                    {
                        Id = u.Id,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Email = u.Email,
                        CompanyId = c.Id
                    })));
        }
    }
}