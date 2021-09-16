using System;
using System.Threading.Tasks;
using Entities.Models;
using Exceptions;
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
            if (method.Equals("POST")) return;
            var customerId = (Guid)context.ActionArguments["customerId"];
            var customer = await _repository.GetByIdAsync(customerId);
            if (customer == null)
            {
                throw new NotFoundException(nameof(Customer), customerId);
            }
            context.HttpContext.Items.Add("customer", customer);
            await next();
        }
    }
}