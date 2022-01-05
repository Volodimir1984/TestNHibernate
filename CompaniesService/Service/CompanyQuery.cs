using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ConstData;
using Microsoft.Extensions.Caching.Distributed;
using NHibernate.Linq;
using ServicesInterfaces.Companies;
using TestBase;
using TestBase.Data;
using TestBaseDto;
using TestBaseDto.Company;

namespace CompaniesService.Service
{
    public class CompanyQuery: ICompanyQuery
    {
        private readonly INHibernateQuerySession _session;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        public CompanyQuery(INHibernateQuerySession session,
            IMapper mapper,
            IDistributedCache cache)
        {
            _session = session;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<IEnumerable<CompanyAllDataDto>> GetCompaniesAsync()
        {
            var key = "Companies";
            var companiesDto = await _cache.GetCacheByKeyAsync<IEnumerable<CompanyAllDataDto>>(key);

            if (companiesDto == null)
            {
                var companies = await _session.Companies.ToListAsync();
                companiesDto = _mapper.Map<IEnumerable<Company>, IEnumerable<CompanyAllDataDto>>(companies);
                await _cache.SetCacheAsync(key, companiesDto);
            }

            return companiesDto;
        }

        public async Task<CompanyAllDataDto> GetCompanyAsync(int id)
        {
            var key = "Company:" + id;

            var companyDto = await _cache.GetCacheByKeyAsync<CompanyAllDataDto>(key);

            if (companyDto == null)
            {
                var company = await _session.Companies.FirstOrDefaultAsync(c => c.Id == id);

                if (company == null)
                    throw new CompanyServiceException(ConstStringForException.NotFoundCompany);

                companyDto = _mapper.Map<CompanyAllDataDto>(company);
                await _cache.SetCacheAsync(key, companyDto);
            }

            return companyDto;
        }

        public async Task<IEnumerable<CompanyAllDataDto>> GetCompanyWithCountUsersAsync(int count)
        {
            var key = "CompanyWithCountUsers:" + count;

            var companiesDto = await _cache.GetCacheByKeyAsync<IEnumerable<CompanyAllDataDto>>(key);

            if (companiesDto == null)
            {
                var companies = await _session.Companies.Where(c => c.Users.Count >= count)
                    .ToListAsync();
                companiesDto = _mapper.Map<List<Company>, IEnumerable<CompanyAllDataDto>>(companies);
                await _cache.SetCacheAsync(key, companiesDto);
            }

            return companiesDto;
        }
    }
}
