using System.Threading.Tasks;
using Xunit;

namespace OrderServiceTests.ValidationTests
{
    public class ProductValidatorTests
    {
        private ProductValidator _validator = new();
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task NullOrEmptyName_ThrowsError(string nameValue)
        {
            var model = new Product { Name = nameValue };
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(a => a.Name);
        }

        [Fact]
        public async Task ProperStringName_NotThrowingError()
        {
            var model = new Product { Name = "A Proper Name" };
            var result = await _validator.TestValidateAsync(model);
            result.ShouldNotHaveValidationErrorFor(a => a.Name);
        }
        
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task NullOrEmptyUrl_ThrowsError(string urlValue)
        {
            var model = new Product { ImageUrl = urlValue };
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(a => a.ImageUrl);
        }

        [Fact]
        public async Task ProperStringUrl_NotThrowingError()
        {
            var model = new Product { ImageUrl = "A Proper Name" };
            var result = await _validator.TestValidateAsync(model);
            result.ShouldNotHaveValidationErrorFor(a => a.ImageUrl);
        }
    }
}