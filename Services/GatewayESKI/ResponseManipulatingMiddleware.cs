using System.Threading.Tasks;
using GatewayESKI.ErrorModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace GatewayESKI
{
    public class ResponseManipulatingMiddleware
    {
        
        private RequestDelegate _next;

        public ResponseManipulatingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            await _next.Invoke(httpContext);
            var response = httpContext.Response;
            switch (response.StatusCode)
            {
                case 404:
                    throw new NotFound();
                case 401:
                    throw new UnAuthorized();
                case 400:
                    throw new BadRequest();
            }
        }
    }
    public static class ResponseManipulatingMiddlewareExtension
    {
        public static IApplicationBuilder UseResponseManipulation(this IApplicationBuilder applicationBuilder) =>
            applicationBuilder.UseMiddleware<ResponseManipulatingMiddleware>();
    }
}