using System;
using System.Threading.Tasks;
using CommonLib.Models.ErrorModels;
using Entities.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Repository;

namespace CustomerService.Filters
{
    public class ValidateCustomerExistAttribute : IAsyncActionFilter
    {
        private IRepository<Customer> _repository;

        public ValidateCustomerExistAttribute(IRepository<Customer> repository)
        {
            _repository = repository;
        }
        
        public async Task OnActionExecutionAsync(ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            var method = context.HttpContext.Request.Method;
            if (context.HttpContext.Request.RouteValues.ContainsKey("customerId"))
            {
                var customerId = (Guid) context.ActionArguments["customerId"];
                var customer = await _repository.GetByIdAsync(customerId);
                if (customer == null) throw new NotFound(nameof(Customer), customerId.ToString());
                context.HttpContext.Items.Add("customer", customer);
            }
            await next();
        }
    }
}