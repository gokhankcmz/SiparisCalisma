using System.Linq;
using Entities.RequestModels;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CustomerService.Filters
{
    public static class EmailProducer
    {
        public static string GetEmail(ActionExecutingContext context)
        {
            var method = context.HttpContext.Request.Method;
            if (method.Equals("POST"))
            {
                var dto = (CreateCustomerDto) context.ActionArguments.First(x => x.Key.ToLower().Contains("dto")).Value;
                return dto.Email;
            }
            if (method.Equals("PUT"))
            {
                var dto = (UpdateCustomerDto) context.ActionArguments.First(x => x.Key.ToLower().Contains("dto")).Value;
                return dto.Email;
            }
            return null;
        }
    }
}