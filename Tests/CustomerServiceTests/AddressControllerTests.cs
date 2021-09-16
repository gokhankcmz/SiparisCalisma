using System;
using System.Threading.Tasks;
using AutoMapper;
using CustomerService.Controllers;
using CustomerService.Utility;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Repository;
using Xunit;

namespace CustomerServiceTests
{
    public class AddressControllerTests
    {
        private readonly Mock<IRepository<Customer>> _mockRepository = new();
        private readonly IMapper _mapper;

        public AddressControllerTests()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }

        [Fact]
        public async Task GetCustomerAddress_ExistingCustomer_ReturnsOkObject()
        {
            _mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new Customer());
            var addressController = new AddressController(_mockRepository.Object, _mapper);
            var result = await addressController.GetCustomerAddress(Guid.NewGuid());

            Assert.IsType<OkObjectResult>(result);
        }
    }
}