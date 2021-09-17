using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using Entities.RequestModels;
using Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using Newtonsoft.Json;
using Repository;

namespace CustomerService.Filters
{
    public class ValidateEmailIsUniqueAttribute : IAsyncActionFilter
    {
        
        private IRepository<Customer> _repository;

        public ValidateEmailIsUniqueAttribute(IRepository<Customer> repository)
        {
            _repository = repository;
        }
        
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var method = context.HttpContext.Request.Method;
            if (method.Equals("POST")|| method.Equals("PUT"))
            {
                var email = EmailProducer.GetEmail(context);
                var customerEntity = (await _repository.GetByCondition(x=> x.Email==email)).FirstOrDefault();
                if (customerEntity != null)
                {
                    throw new EmailIsNotUniqueException(nameof(Customer), email);
                }
            }
            await next();

        }

 
    }
}