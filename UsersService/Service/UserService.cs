using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ConstData;
using Microsoft.Extensions.Caching.Distributed;
using NHibernate.Linq;
using ServicesInterfaces.Users;
using TestBase;
using TestBase.Data;
using TestBaseDto;

namespace UsersService.Service
{
    public class UserService : IUserService
    {
        private readonly INHibernateSession _session;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        public UserService(INHibernateSession session,
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

        public async Task UpdateUserAsync(UserDto user)
        {
            try
            {
                _session.BeginTransaction();
                var updateUser = _mapper.Map<AspNetUsers>(user);
                 
                await _session.SaveAsync(updateUser);
                await _session.CommitAsync();
            }
            catch (Exception e)
            {
                try
                {
                    await _session.RollbackAsync();
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                throw new Exception(e.Message);
            }
            finally
            {
                _session.CloseTransaction();
            }
        }

        public async Task DeleteUserAsync(int userId)
        {
            try
            {
                _session.BeginTransaction();
                var deleteUser = await _session.Users.FirstOrDefaultAsync(u => u.Id == userId);

                if (deleteUser == null)
                    throw new UserServiceException(ConstStringForException.NotFoundUser);

                await _session.DeleteAsync(deleteUser);
                await _session.CommitAsync();
            }
            catch (Exception e)
            {
                try
                {
                    await _session.RollbackAsync();
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                throw new Exception(e.Message);
            }
            finally
            {
                _session.CloseTransaction();
            }

        }

        public async Task CreateUserAsync(UserDto user)
        {
            try
            {
                _session.BeginTransaction();
               
                var createCompany = _mapper.Map<AspNetUsers>(user);
                
                await _session.SaveAsync(createCompany);
                await _session.CommitAsync();
            }
            catch (Exception e)
            {
                try
                {
                    await _session.RollbackAsync();
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                throw new Exception(e.Message);
            }
            finally
            {
                _session.CloseTransaction();
            }
        }
    }
}