using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Entities.Models;
using Entities.RequestModels;
using Entities.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using OrderService.Filters;

namespace OrderService.Controllers
{
    [Route("orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private IApplicationService _applicationService;
        private IMapper _mapper;

        public OrdersController(IApplicationService applicationService, IMapper mapper)
        {
            _applicationService = applicationService;
            _mapper = mapper;
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidateCustomerExistAttribute))]
        public async Task<IActionResult> CreateOrder([FromBody]CreateOrderDto createOrderDto)
        {
            var orderEntity = _mapper.Map<Order>(createOrderDto);
            orderEntity.Address  = HttpContext.Items["address"] as Address;
            await _applicationService.CreateAsync(orderEntity);
             return CreatedAtRoute("OrderById", new { orderId = orderEntity.Id },
                 _mapper.Map<OrderResponseDto>(orderEntity));
        }


        [HttpGet("{orderId:guid}", Name = "OrderById")]
        [ServiceFilter(typeof(ValidateOrderExistAttribute))]
        public async Task<IActionResult> GetOrder(Guid orderId)
        {
            var orderEntity = await _applicationService.GetAsync(orderId);
            _applicationService.CheckIfSelfOrder(orderEntity, (Guid) HttpContext.Items["customerId"]);
            return Ok(_mapper.Map<OrderResponseDto>(orderEntity));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _applicationService.GetAllAsync();
            return Ok(_mapper.Map<List<OrderCollectionDto>>(orders));
        }

        [HttpPut("{orderId:guid}")]
        [ServiceFilter(typeof(ValidateOrderExistAttribute))]
        public async Task<IActionResult> UpdateOrder(Guid orderId, [FromBody] UpdateOrderDto updateOrderDto)
        {
            var orderEntity = await _applicationService.GetAsync(orderId);
            _applicationService.CheckIfSelfOrder(orderEntity, (Guid) HttpContext.Items["customerId"]);
            _mapper.Map(updateOrderDto, orderEntity);
            await _applicationService.ReplaceAsync(orderEntity);
            return Ok(_mapper.Map<OrderResponseDto>(orderEntity));
        }


        [HttpDelete("{orderId:guid}")]
        [ServiceFilter(typeof(ValidateOrderExistAttribute))]
        public async Task<IActionResult> DeleteOrder(Guid orderId)
        {
            var orderEntity = await _applicationService.GetAsync(orderId);
            _applicationService.CheckIfSelfOrder(orderEntity, (Guid) HttpContext.Items["customerId"]);
            _applicationService.Delete(orderEntity);
            //return Ok(_mapper.Map<OrderResponseDto>(orderEntity));
            return Ok();
        }

    }
}