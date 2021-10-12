using System.Threading.Tasks;
using CommonLib.Helpers.Jwt;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace OrderService
{
    public class CustomerTokenValidationMiddleware
    {
        private RequestDelegate _next;

        public CustomerTokenValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var request = httpContext.Request;
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
        public static IApplicationBuilder UseCustomerTokenValidation(this IApplicationBuilder applicationBuilder) =>
            applicationBuilder.UseMiddleware<CustomerTokenValidationMiddleware>();
    }

}