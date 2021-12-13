using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Services.Company;
using ServicesInterfaces;

namespace TestNHibernate
{
    public class ExceptionFilter: IActionFilter, IOrderedFilter
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(CompanyService));
        public int Order { get; } = int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is IServiceException companyException)
            {
                context.Result = new ObjectResult(companyException.ErrorMessage)
                {
                    StatusCode = 500,
                };
                context.ExceptionHandled = true;
            }
            else if (context.Exception != null)
            {
                _logger.Error(context.Exception.Message);
                context.Result = new ObjectResult(context.Exception.Message)
                {
                    StatusCode = 500,
                };
                context.ExceptionHandled = true;
            }
        }
    }
}