using System;
using System.Threading.Tasks;
using CommonLib.Helpers.Jwt;
using CommonLib.Models.ErrorModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace CustomerService
{
    public class CustomerValidationMiddleware
    {
        private RequestDelegate _next;

        public CustomerValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var request = httpContext.Request;
            if (request.RouteValues.ContainsKey("customerId"))
            {
                var customerIdFromToken = Guid.Parse(request.Headers.GetClaimOrThrow("nameid"));
                var customerIdFromRoute = Guid.Parse(request.RouteValues["customerId"].ToString());
                if (!customerIdFromRoute.Equals(customerIdFromToken)) throw new UnAuthorized();
            }
            await _next.Invoke(httpContext);
        }


    }

    public static class CustomerIdFromTokenMiddlewareExtension
    {
        public static IApplicationBuilder UseCustomerValidation(this IApplicationBuilder applicationBuilder) =>
            applicationBuilder.UseMiddleware<CustomerValidationMiddleware>();
    }

}