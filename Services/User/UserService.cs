using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ConstData;
using NHibernate.Linq;
using ServicesInterfaces;
using TestBase;
using TestBase.Data;
using TestBaseDto;

namespace Services.User
{
    public class UserService: IUserService
    {
        private readonly INHibernateSession _session;
        private readonly IMapper _mapper;

        public UserService(INHibernateSession session, IMapper mapper)
        {
            _session = session;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            var users = await _session.Users.ToListAsync();
            return _mapper.Map<List<AspNetUsers>, IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto> GetUserAsync(int id)
        {
            var user = await _session.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                throw new ServiceException(ConstStringForException.NotFoundUser);
            
            return _mapper.Map<AspNetUsers, UserDto>(user);
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
                    throw new ServiceException(ConstStringForException.NotFoundUser);

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