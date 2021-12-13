using AutoMapper;
using TestBase.Data;

namespace TestBaseDto.Profeilers
{
    public class UserDtoProfiler: Profile
    {
        public UserDtoProfiler()
        {
            CreateMap<AspNetUsers, UserDto>()
                .ForMember(dto => dto.CompanyId, entity => entity.MapFrom(c => c.Company.Id));

            CreateMap<UserDto, AspNetUsers>()
                .ForMember(entity => entity.Company, dto => dto.MapFrom(c => new Company
                {
                    Id = c.CompanyId
                }));
        }
    }
}