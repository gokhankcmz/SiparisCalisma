using System;
using System.Threading.Tasks;
using CommonLib.Helpers.Jwt;
using CommonLib.Middlewares;
using CommonLib.Models.ErrorModels;
using Entities.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace OrderService
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
            var uri = request.GetDisplayUrl();
            if (uri.Contains("orders/"))
            {
                var idFromToken = request.Headers.GetClaimOrThrow("nameid");
                if (!uri.Contains(idFromToken))
                {
                    throw new UnAuthorized(nameof(Customer), idFromToken);
                }
            }
            await _next.Invoke(httpContext);
        }


    }

    public static class CustomerValidationMiddlewareExtension
    {
        public static IApplicationBuilder UseCustomerValidator(this IApplicationBuilder applicationBuilder) =>
            applicationBuilder.UseMiddleware<CustomerValidationMiddleware>();
    }

}