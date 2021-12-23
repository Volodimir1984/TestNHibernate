using System;
using ServicesInterfaces;

namespace CompaniesService.Service
{
    public class CompanyServiceException: Exception, IServiceException
    {
        public CompanyServiceException(string message): base(message)
        {
            ErrorMessage = message;
        }
        
        public string ErrorMessage { get; set; }
    }
}