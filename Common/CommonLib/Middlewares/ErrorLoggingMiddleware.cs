using System;
using System.Threading.Tasks;
using CommonLib.Models.ErrorModels;
using Serilog;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace CommonLib.Middlewares
{
    public class ErrorLoggingMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;

        public ErrorLoggingMiddleware(ILogger logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ErrorDetails e)
            {
                _logger.Information(e, "A known error has occurred.");
            }
            catch (Exception e)
            {
                _logger.Error(e, "An unknown error has occured.");
            }
        }

    }
    
    public static class ErrorLoggingMiddleWareExtension
    {
        public static IApplicationBuilder UseErrorLogger(this IApplicationBuilder applicationBuilder) =>
            applicationBuilder.UseMiddleware<ErrorLoggingMiddleware>();
    }
}