using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using TestBase.Data;

namespace TestBaseDto.Profeilers
{
    public class CompanyDtoProfiler: Profile
    {
        public CompanyDtoProfiler()
        {
            CreateMap<Company, CompanyDto>()
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
            
            CreateMap<CompanyDto, Company>()
                .ForMember(entity => entity.Users, dto => dto.Ignore());
        }
    }
}