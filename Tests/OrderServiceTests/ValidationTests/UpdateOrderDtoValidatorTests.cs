using System;
using System.Threading.Tasks;
using Entities.RequestModels;
using FluentValidation.TestHelper;
using OrderService.ValidationRules;
using Xunit;

namespace OrderServiceTests.ValidationTests
{
    public class UpdateOrderDtoValidatorTests
    {
        
        private UpdateOrderDtoValidator _validator = new();
    
        [Theory]
        [InlineData(null)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        public async Task Null_Negative_OrPositiveMax_Quantity_ThrowsError(int priceValue)
        {
            var model = new UpdateOrderDto { Quantity = priceValue };
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(a => a.Quantity);
        }

        [Fact]
        public async Task ProperQuantity_NotThrowingError()
        {
            var model = new UpdateOrderDto { Quantity = new Random().Next(0,998)};
            var result = await _validator.TestValidateAsync(model);
            result.ShouldNotHaveValidationErrorFor(a => a.Quantity);
        }
        
        
        [Theory]
        [InlineData(null)]
        [InlineData(-1)]
        [InlineData(99999999)]
        public async Task Null_Negative_OrPositiveMax_Price_ThrowsError(decimal priceValue)
        {
            var model = new UpdateOrderDto { Price = priceValue };
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(a => a.Price);
        }

        [Fact]
        public async Task ProperPrice_NotThrowingError()
        {
            var model = new UpdateOrderDto { Price = new Random().Next(0,99998)};
            var result = await _validator.TestValidateAsync(model);
            result.ShouldNotHaveValidationErrorFor(a => a.Price);
        }

    }
}