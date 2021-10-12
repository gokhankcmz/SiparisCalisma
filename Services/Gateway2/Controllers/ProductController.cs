using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Gateway2.Controllers
{
    [Route("orders/{orderId}/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private Service _orderService;

        public ProductController()
        {
            _orderService = new Service("orderservice", 80);
        }
        

        [HttpGet]
        public async Task<IActionResult> GetOrderProducts(Guid orderId)
        {
            var (response, _) = await _orderService.Route(HttpContext);
            return Ok(response);
        }
    }
}