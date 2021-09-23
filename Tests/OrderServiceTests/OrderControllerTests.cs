using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace OrderServiceTests
{
    public class OrderControllerTests
    {
        
        private readonly Mock<IRepository<Order>> _mockRepository = new();
        private readonly IMapper _mapper;
        
        public OrderControllerTests()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
            
        }
        
        //Get Tests.
        [Fact]
        public async Task GetOrder_ExistingId_ReturnsOkObject()
        {
            _mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new Order());
            var ordersController = new OrdersController(_mockRepository.Object, _mapper);
            var result = await ordersController.GetOrder(Guid.NewGuid());

            Assert.IsType<OkObjectResult>(result);
        }
        
        [Fact]
        public async Task GetAllOrders_ReturnsOkObject()
        {
            _mockRepository.Setup(r => r.GetAll())
                .ReturnsAsync(new List<Order>());
            var controller = new OrdersController(_mockRepository.Object, _mapper);
            var result = await controller.GetAllOrders();

            Assert.IsType<OkObjectResult>(result);

        }
        
        // Delete Test
        [Fact]
        public async Task DeleteOrder_ReturnsOkObject()
        {
            _mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new Order());
            _mockRepository.Setup(r => r.Delete(It.IsAny<Order>()));
            var ordersController = new OrdersController(_mockRepository.Object, _mapper);
            var result = await ordersController.DeleteOrder(Guid.NewGuid());

            Assert.IsType<OkObjectResult>(result);
        }
        
        // Update Test
        [Fact]
        public async Task UpdateOrder_ReturnsOkObject()
        {
            _mockRepository.Setup(x => x.CreateAsync(It.IsAny<Order>()))
                .ReturnsAsync(It.IsAny<Order>);
            _mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(It.IsAny<Order>);
            var controller = new OrdersController(_mockRepository.Object, _mapper);
            
            var result = await controller.UpdateOrder(It.IsAny<Guid>(),Mock.Of<UpdateOrderDto>());

            Assert.IsType<OkObjectResult>(result);
        }
    }
}