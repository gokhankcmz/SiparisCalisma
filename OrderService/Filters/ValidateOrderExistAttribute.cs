using System;
using System.Threading.Tasks;
using Entities.Models;
using Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using Repository;

namespace OrderService.Filters
{
    public class ValidateOrderExistAttribute : IAsyncActionFilter
    {
        private IRepository<Order> _repository;

        public ValidateOrderExistAttribute(IRepository<Order> repository)
        {
            _repository = repository;
        }
        
        public async Task OnActionExecutionAsync(ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            var method = context.HttpContext.Request.Method;
            if (method.Equals("POST")) return;
            var orderId = (Guid)context.ActionArguments["orderId"];
            var order = await _repository.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new NotFoundException(nameof(Order), orderId);
            }
            context.HttpContext.Items.Add("order", order);
            await next();
        }
    }
}