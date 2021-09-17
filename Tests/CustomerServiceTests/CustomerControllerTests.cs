using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using CustomerService.Controllers;
using CustomerService.Utility;
using Entities.Models;
using Entities.RequestModels;
using Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Repository;
using Xunit;

namespace CustomerServiceTests
{
    public class CustomerControllerTests
    {

        private readonly Mock<IRepository<Customer>> _mockRepository = new();
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
            _mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new Customer());
            var customersController = new CustomersController(_mockRepository.Object, _mapper);
            var result = await customersController.GetCustomer(Guid.NewGuid());

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetAllCustomers_ReturnsOkObject()
        {
            _mockRepository.Setup(r => r.GetAll())
                .ReturnsAsync(new List<Customer>());
            var controller = new CustomersController(_mockRepository.Object, _mapper);
            var result = await controller.GetAllCustomers();

            Assert.IsType<OkObjectResult>(result);

        }

        
        [Fact]
        public async Task DeleteCustomer_ExistingCustomerId_ReturnsOkObject()
        {
            _mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new Customer());
            _mockRepository.Setup(r => r.Delete(It.IsAny<Customer>()));
            var customersController = new CustomersController(_mockRepository.Object, _mapper);
            var result = await customersController.DeleteCustomer(Guid.NewGuid());

            Assert.IsType<OkObjectResult>(result);
        }

    }
}