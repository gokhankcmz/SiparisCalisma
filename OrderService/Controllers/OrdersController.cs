using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Entities.Models;
using Entities.RequestModels;
using Entities.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using OrderService.Filters;
using Repository;

namespace OrderService.Controllers
{
    [Route("orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private IRepository<Order> _repository;
        private IMapper _mapper;

        public OrdersController(IRepository<Order> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidateCustomerExistAttribute))]
        public async Task<IActionResult> CreateOrder([FromBody]CreateOrderDto createOrderDto)
        {
            var orderEntity = _mapper.Map<Order>(createOrderDto);
            orderEntity.Address  = HttpContext.Items["address"] as Address;
            await _repository.CreateAsync(orderEntity);
             return CreatedAtRoute("OrderById", new { orderId = orderEntity.Id },
                 _mapper.Map<OrderResponseDto>(orderEntity));
        }


        [HttpGet("{orderId:guid}", Name = "OrderById")]
        [ServiceFilter(typeof(ValidateOrderExistAttribute))]
        public async Task<IActionResult> GetOrder(Guid orderId)
        {
            var orderEntity = await _repository.GetByIdAsync(orderId);
            return Ok(_mapper.Map<OrderResponseDto>(orderEntity));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _repository.GetAll();
            return Ok(_mapper.Map<List<OrderCollectionDto>>(orders));
        }

        [HttpPut("{orderId:guid}")]
        [ServiceFilter(typeof(ValidateOrderExistAttribute))]
        public async Task<IActionResult> UpdateOrder(Guid orderId, [FromBody] UpdateOrderDto updateOrderDto)
        {
            var orderEntity = await _repository.GetByIdAsync(orderId);
            _mapper.Map(updateOrderDto, orderEntity);
            await _repository.ReplaceAsync(orderEntity);
            return Ok(_mapper.Map<OrderResponseDto>(orderEntity));
        }


        [HttpDelete("{orderId:guid}")]
        [ServiceFilter(typeof(ValidateOrderExistAttribute))]
        public async Task<IActionResult> DeleteOrder(Guid orderId)
        {
            var orderEntity = await _repository.GetByIdAsync(orderId);
            _repository.Delete(orderEntity);
            return Ok(_mapper.Map<OrderResponseDto>(orderEntity));
        }

    }
}