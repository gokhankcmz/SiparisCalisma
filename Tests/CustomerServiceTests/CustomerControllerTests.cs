using System;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using CustomerService.Controllers;
using CustomerService.Filters;
using CustomerService.Utility;
using Entities.Models;
using Entities.RequestModels;
using FluentAssertions;
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
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new MappingProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }
        
        
        [Fact]
        public async Task GetCustomer_ExistingId_ReturnsOkObject()
        {
            _mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new Customer());
            var customersController = new CustomersController(_mockRepository.Object, _mapper);
            var result = await customersController.GetCustomer(Guid.NewGuid());
            
            Assert.IsType<OkObjectResult>(result);
        }

        // [Fact]
        // public async Task CreateCustomer_RegisteredEmail_ReturnsConflict()
        // {
        //     _mockRepository.Setup(r => r.CreateAsync(It.IsAny<Customer>()));
        // }
    }
}