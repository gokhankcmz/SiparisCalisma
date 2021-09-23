using System.Linq;
using System.Threading.Tasks;
using CommonLib.Models.ErrorModels;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CustomerService.Filters
{
    public class ValidateEmailIsUniqueAttribute : IAsyncActionFilter
    {
        
        private IApplicationService _applicationService;

        public ValidateEmailIsUniqueAttribute(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }
        
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var method = context.HttpContext.Request.Method;
            if (method.Equals("POST") || method.Equals("PUT"))
            {
                var email = EmailProducer.GetEmail(context);
                var customerEntity = (await _applicationService.GetByConditionAsync(x=> x.Email==email)).FirstOrDefault();
                if (customerEntity != null)
                {
                    //throw new EmailIsNotUniqueException(nameof(Customer), email);
                    throw new Conflict("Customer", email);
                }
            }
            await next();

        }

 
    }
}