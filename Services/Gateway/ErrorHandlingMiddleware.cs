using System;
using System.Threading.Tasks;
using Gateway.ErrorModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace Gateway
{
    public class ErrorHandlingMiddleware
    {
        private readonly ILogger _logger;
        private RequestDelegate _next;

        public ErrorHandlingMiddleware(ILogger logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next.Invoke(httpContext);
            }
            catch (ErrorDetails ex)
            {
                // if (typeof(ErrorDetails) == ex.GetType())
                // {
                //     
                // }
                _logger.Information(ex.GetBaseException(),"A known error has occurred.");
                _logger.Information(ex, "A known error has occurred.");
                await HandleAsync(httpContext, ex);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An unknown error has occured.");
                await HandleAsync(httpContext, ex);
            }
        }

        public async Task HandleAsync(HttpContext httpContext, ErrorDetails exception)
        {
            var response = httpContext.Response;
            response.ContentType = "application/json";
            response.StatusCode = exception.StatusCode;
            
            await response.WriteAsync(exception.ToString());

        }

        public async Task HandleAsync(HttpContext httpContext, System.Exception exception)
        {
            var response = httpContext.Response;
            response.ContentType = "application/json";
            response.StatusCode = 500;
            await response.WriteAsync(exception.Message.ToString());
        }
    }

    public static class ErrorHandlingMiddleWareExtension
    {
        public static IApplicationBuilder UseCustomErrorHandler(this IApplicationBuilder applicationBuilder) =>
            applicationBuilder.UseMiddleware<ErrorHandlingMiddleware>();
    }

}