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
    public class CustomerIdFromTokenMiddleware
    {
        private RequestDelegate _next;

        public CustomerIdFromTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var request = httpContext.Request;
            var uri = request.GetDisplayUrl();

            if (request.RouteValues.ContainsKey("orderId"))
            {
                var idFromToken = request.Headers.GetClaimOrThrow("nameid");
                httpContext.Items.Add("customerId", idFromToken);
                
            }
            await _next.Invoke(httpContext);
        }


    }

    public static class CustomerIdFromTokenMiddlewareExtension
    {
        public static IApplicationBuilder UseCustomerIdFromToken(this IApplicationBuilder applicationBuilder) =>
            applicationBuilder.UseMiddleware<CustomerIdFromTokenMiddleware>();
    }

}