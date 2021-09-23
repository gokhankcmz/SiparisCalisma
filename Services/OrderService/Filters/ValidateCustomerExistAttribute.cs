using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CommonLib.Models.ErrorModels;
using Entities.Models;
using Entities.RequestModels;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace OrderService.Filters
{
    public class ValidateCustomerExistAttribute : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            
            var customerId = ((CreateOrderDto)context.ActionArguments["createOrderDto"]).CustomerId;
            HttpClient client = new HttpClient();
            var res =  await client.GetAsync($"http://customerservice:80/customers/{customerId}");
            if (res.StatusCode == HttpStatusCode.NotFound)
            {
                throw new NotFound("Customer", customerId.ToString());
            }
            var contentResult = res.Content.ReadAsStringAsync().Result;
            var address = JsonConvert.DeserializeObject<Customer>(contentResult).Address;
            context.HttpContext.Items.Add("address", address);
            await next();
        }
    }
}