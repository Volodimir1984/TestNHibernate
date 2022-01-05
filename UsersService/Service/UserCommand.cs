using AutoMapper;
using ConstData;
using Microsoft.Extensions.Caching.Distributed;
using NHibernate.Linq;
using ServicesInterfaces.Users;
using System;
using System.Threading.Tasks;
using TestBase;
using TestBase.Data;
using TestBaseDto.User;

namespace UsersService.Service
{
    public class UserCommand : IUserCommand
    {
        private readonly INHibernateSession _session;
        private readonly IMapper _mapper;

        public UserCommand(INHibernateSession session,
            IMapper mapper)
        {
            _session = session;
            _mapper = mapper;
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