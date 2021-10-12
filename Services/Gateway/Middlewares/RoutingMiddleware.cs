using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CommonLib.Models.ErrorModels;
using Gateway.Models;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace Gateway.Middlewares
{
    public class RoutingMiddleware
    {
        private ILogger _logger;
        private List<Service> _services;
        private RequestDelegate _next;

        public RoutingMiddleware(ILogger logger, List<Service> services, RequestDelegate next)
        {
            _logger = logger;
            _services = services;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            
            await _next.Invoke(context);
            var inComingRequest = context.Request;
            var sendingRequest = new HttpRequestMessage();
            
            //Finding Matching Service
            if (inComingRequest.Path.Value == null) throw new NotFound();
            var targetService = _services.FirstOrDefault(x => x.RoutingKeys.Any(inComingRequest.Path.Value.Contains));
            
            //Recreating Target Uri
            if (targetService == null) throw new NotFound();
            sendingRequest.RequestUri = new Uri($"http://{targetService.HostName}:{targetService.Port}{inComingRequest.Path.Value}");
            
            

            //Copy incoming request method to sending request method.
            sendingRequest.Method = new HttpMethod(inComingRequest.Method);
                
                
            //Copy incoming request body to sending request body.
            var sr = new StreamReader(inComingRequest.Body);
            sendingRequest.Content = new StringContent(await sr.ReadToEndAsync());
            
            
            //Create new htttpclient and send request.
            var client = new HttpClient();
            var result = await client.SendAsync(sendingRequest);

            context.Response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            
            //Copy incomingResponse body to response
            //await result.Content.ReadAsStreamAsync().Result.CopyToAsync(context.Response.Body);
            context.Response.StatusCode = (int) result.StatusCode;
            await context.Response.WriteAsync(result.Content.ReadAsStringAsync().Result);

            //Copy Status Code
            //context.Response.StatusCode = (int) result.StatusCode;
            
        }
    }
}