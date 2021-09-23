using System;
using System.Threading.Tasks;
using CommonLib.Models.ErrorModels;
using Entities.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CustomerService.Filters
{
    public class ValidateCustomerExistAttribute : IAsyncActionFilter
    {
        private IApplicationService _applicationService;

        public ValidateCustomerExistAttribute(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }
        
        public async Task OnActionExecutionAsync(ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            var method = context.HttpContext.Request.Method;
            if (context.HttpContext.Request.RouteValues.ContainsKey("customerId"))
            {
                var customerId = (Guid) context.ActionArguments["customerId"];
                var customer = await _applicationService.GetAsync(customerId);
                if (customer == null) throw new NotFound(nameof(Customer), customerId.ToString());
                context.HttpContext.Items.Add("customer", customer);
            }
            await next();
        }
    }
}