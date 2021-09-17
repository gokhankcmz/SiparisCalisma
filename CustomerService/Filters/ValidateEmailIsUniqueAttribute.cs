using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using Newtonsoft.Json;
using Repository;

namespace CustomerService.Filters
{
    /*public class ValidateEmailIsUniqueAttribute : IAsyncActionFilter
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
                //var body = context.HttpContext?.Request?.BodyToString();
                //var dtoValueRaw =  (string) context.ActionArguments.First(x=> x.Key.ToLower().Contains("dto")).Value;
                var props = JsonConvert.DeserializeObject<Dictionary<string, string>>(body);
                var emailValue = props.First(v => v.Key.ToLower().Equals("email")).Value;
                var customerEntity = (await _repository.GetByCondition(x=> x.Email==emailValue)).FirstOrDefault();
                if (customerEntity != null)
                {
                    throw new EmailIsNotUniqueException(nameof(Customer), emailValue);
                }
            }
            await next();

        }

 
    }*/
}