using System.Threading.Tasks;
using CustomerService.ValidationRules;
using Entities.RequestModels;
using FluentValidation.TestHelper;
using Xunit;

namespace CustomerServiceTests.ValidationTests
{
    public class UpdateCustomerDtoValidatorTests
    {
        private UpdateCustomerDtoValidator _validator = new();
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("aStringWithoutAtSign.com")]
        public async Task NullOrEmptyOrRegularStringEmail_ThrowsError(string email)
        {
            var model = new UpdateCustomerDto { Email = email };
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(c => c.Email);
        }
        
        [Fact]
        public async Task ProperStringEmail_NotThrowingError()
        {
            var model = new UpdateCustomerDto { Email = "test@email.com" };
            var result = await _validator.TestValidateAsync(model);
            result.ShouldNotHaveValidationErrorFor(c => c.Email);
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("aStringWithNumber123")]
        //[InlineData("aStringWithout@DotCom")]
        //[InlineData("aStringWithSpace @email.com")]
        //[InlineData("aStringWithNotAcceptableChar/@email.com")]
        public async Task NullOrEmptyOrDigitStringName_ThrowsError(string name)
        {
            var model = new UpdateCustomerDto { Name = name };
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(c => c.Name);
        }
        
        [Fact]
        public async Task ProperStringName_NotThrowingError()
        {
            var model = new UpdateCustomerDto { Name = "Example Name" };
            var result = await _validator.TestValidateAsync(model);
            result.ShouldNotHaveValidationErrorFor(c => c.Name);
        }
    }
}