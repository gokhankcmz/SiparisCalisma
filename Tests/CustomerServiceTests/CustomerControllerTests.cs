using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CustomerServiceTests
{
    public class CustomerControllerTests
    {

        private readonly Mock<IApplicationService> _mockApplicationService = new();
        private readonly IMapper _mapper;

        public CustomerControllerTests()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }

        // Get Tests
        [Fact]
        public async Task GetCustomer_ExistingId_ReturnsOkObject()
        {
            _mockApplicationService.Setup(r => r.GetAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new Customer());
            var customersController = new CustomersController(_mockApplicationService.Object, _mapper);
            var result = await customersController.GetCustomer(Guid.NewGuid());

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetAllCustomers_ReturnsOkObject()
        {
            _mockApplicationService.Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<Customer>());
            var controller = new CustomersController(_mockApplicationService.Object, _mapper);
            var result = await controller.GetAllCustomers();

            Assert.IsType<OkObjectResult>(result);

        }

        
        [Fact]
        public async Task DeleteCustomer_ExistingCustomerId_ReturnsOkObject()
        {
            _mockApplicationService.Setup(r => r.GetAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new Customer());
            _mockApplicationService.Setup(r => r.Delete(It.IsAny<Customer>()));
            var customersController = new CustomersController(_mockApplicationService.Object, _mapper);
            var result = await customersController.DeleteCustomer(Guid.NewGuid());

            Assert.IsType<OkObjectResult>(result);
        }

    }
}