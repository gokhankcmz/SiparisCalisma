using System;
using System.Threading.Tasks;
using Entities.RequestModels;
using FluentValidation.TestHelper;
using OrderService.ValidationRules;
using Xunit;

namespace OrderServiceTests.ValidationTests
{
    public class CreateOrderDtoValidatorTests
    {
        private CreateOrderDtoValidator _validator = new();
    
        [Theory]
        [InlineData(null)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        public async Task Null_Negative_OrPositiveMax_Quantity_ThrowsError(int priceValue)
        {
            var model = new CreateOrderDto { Quantity = priceValue };
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(a => a.Quantity);
        }

        [Fact]
        public async Task ProperQuantity_NotThrowingError()
        {
            var model = new CreateOrderDto { Quantity = new Random().Next(0,998)};
            var result = await _validator.TestValidateAsync(model);
            result.ShouldNotHaveValidationErrorFor(a => a.Quantity);
        }
        
        
        [Theory]
        [InlineData(null)]
        [InlineData(-1)]
        [InlineData(99999999)]
        public async Task Null_Negative_OrPositiveMax_Price_ThrowsError(decimal priceValue)
        {
            var model = new CreateOrderDto { Price = priceValue };
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(a => a.Price);
        }

        [Fact]
        public async Task ProperPrice_NotThrowingError()
        {
            var model = new CreateOrderDto { Price = new Random().Next(0,99998)};
            var result = await _validator.TestValidateAsync(model);
            result.ShouldNotHaveValidationErrorFor(a => a.Price);
        }

    }
}