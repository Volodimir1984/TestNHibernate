using System;
using ServicesInterfaces;

namespace Services
{
    public class ServiceException: Exception, IServiceException
    {
        public ServiceException(string message): base(message)
        {
            ErrorMessage = message;
        }
        
        public string ErrorMessage { get; set; }
    }
}