using AutoMapper;
using TestBase.Data;
using TestBaseDto.User;

namespace TestBaseDto.Profeilers.User
{
    public class UserDtoProfiler: Profile
    {
        public UserDtoProfiler()
        {
            CreateMap<AspNetUsers, UserDto>()
                .ForMember(dto => dto.CompanyId, entity => entity.MapFrom(c => c.Company.Id));

            CreateMap<UserDto, AspNetUsers>()
                .ForMember(entity => entity.Company, dto => dto.MapFrom(c => new TestBase.Data.Company
                {
                    Id = c.CompanyId
                }));
        }
    }
}