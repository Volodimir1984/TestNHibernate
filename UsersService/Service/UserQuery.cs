using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ConstData;
using Microsoft.Extensions.Caching.Distributed;
using NHibernate.Linq;
using ServicesInterfaces.Users;
using TestBase;
using TestBase.Data;
using TestBaseDto;
using TestBaseDto.User;

namespace UsersService.Service
{
    public class UserQuery: IUserQuery
    {
        private readonly INHibernateQuerySession _session;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        public UserQuery(INHibernateQuerySession session,
            IMapper mapper,
            IDistributedCache cache)
        {
            _session = session;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            var key = "Users";

            var usersDto = await _cache.GetCacheByKeyAsync<IEnumerable<UserDto>>(key);

            if (usersDto == null)
            {
                var users = await _session.Users.ToListAsync();
                usersDto = _mapper.Map<List<AspNetUsers>, IEnumerable<UserDto>>(users);
                await _cache.SetCacheAsync(key, usersDto);
            }

            return usersDto;
        }

        public async Task<UserDto> GetUserAsync(int id)
        {
            var key = "User:" + id;

            var userDto = await _cache.GetCacheByKeyAsync<UserDto>(key);

            if (userDto == null)
            {
                var user = await _session.Users.FirstOrDefaultAsync(u => u.Id == id);

                if (user == null)
                    throw new UserServiceException(ConstStringForException.NotFoundUser);

                userDto = _mapper.Map<AspNetUsers, UserDto>(user);
                await _cache.SetCacheAsync(key, userDto);
            }

            return userDto;
        }
    }
}
