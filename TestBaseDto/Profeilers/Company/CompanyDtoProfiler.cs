using AutoMapper;
using TestBaseDto.Company;

namespace TestBaseDto.Profeilers.Company
{
    public class CompanyDtoProfiler: Profile
    {
        public CompanyDtoProfiler()
        {
            CreateMap<CompanyDto, TestBase.Data.Company>()
                .ForMember(dto => dto.Adress, entity => entity.MapFrom(c => c.Address))
                .ForMember(entity => entity.Users, dto => dto.Ignore());
        }
    }
}
