using System;
using System.Threading.Tasks;
using AutoMapper;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OrderService.Controllers;
using OrderService.Utility;
using Repository;
using Xunit;

namespace OrderServiceTests
{
    public class ProductControllerTests
    {
        private readonly Mock<IRepository<Order>> _mockRepository = new();

        [Fact]
        public async Task GetOrderAddress_ExistingOrder_ReturnsOkObject()
        {
            _mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new Order());
            var addressController = new ProductController(_mockRepository.Object);
            var result = await addressController.GetOrderProducts(Guid.NewGuid());

            Assert.IsType<OkObjectResult>(result);
        }
    }
    
}