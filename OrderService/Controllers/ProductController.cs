using System;
using System.Threading.Tasks;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using OrderService.Filters;
using Repository;

namespace OrderService.Controllers
{
    [Route("orders/{orderId}/product")]
    [ApiController]
    [ServiceFilter(typeof(ValidateOrderExistAttribute))]
    public class ProductController : ControllerBase
    {
        
        private IRepository<Order> _repository;

        public ProductController(IRepository<Order> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderProducts(Guid orderId)
        {
            var order = await _repository.GetByIdAsync(orderId);
            return Ok(order.Product);
        }
    }
}