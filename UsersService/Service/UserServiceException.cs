using System;
using ServicesInterfaces;

namespace UsersService.Service
{
    public class UserServiceException: Exception, IServiceException
    {
        public UserServiceException(string message): base(message)
        {
            ErrorMessage = message;
        }
        
        public string ErrorMessage { get; set; }
    }
}