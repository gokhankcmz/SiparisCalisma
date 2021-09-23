using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CustomerServiceTests
{
    public class AddressControllerTests
    {
        private readonly Mock<IApplicationService> _applicationService = new();
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
            _applicationService.Setup(r => r.GetAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new Customer());
            var addressController = new AddressController(_applicationService.Object, _mapper);
            var result = await addressController.GetCustomerAddress(Guid.NewGuid());

            Assert.IsType<OkObjectResult>(result);
        }
    }
}