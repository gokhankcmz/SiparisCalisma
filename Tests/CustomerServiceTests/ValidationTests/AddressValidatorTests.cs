using System;
using System.Threading.Tasks;
using CustomerService.ValidationRules;
using Entities.Models;
using FluentValidation.TestHelper;
using Xunit;

namespace CustomerServiceTests.ValidationTests
{
    public class AddressValidatorTests
    {
        private AddressValidator _validator = new();

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task NullOrEmptyCity_ThrowsError(string cityValue)
        {
            var model = new Address { City = cityValue };
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(a => a.City);
        }

        [Fact]
        public async Task ProperStringCity_NotThrowingError()
        {
            var model = new Address { City = "A Proper City Name" };
            var result = await _validator.TestValidateAsync(model);
            result.ShouldNotHaveValidationErrorFor(a => a.City);
        }
        
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task NullOrEmptyCountry_ThrowsError(string countryValue)
        {
            var model = new Address { Country = countryValue };
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(a => a.Country);
        }

        [Fact]
        public async Task ProperStringCountry_NotThrowingError()
        {
            var model = new Address { Country = "A Proper Country Name" };
            var result = await _validator.TestValidateAsync(model);
            result.ShouldNotHaveValidationErrorFor(a => a.Country);
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task NullOrEmptyAddressLine_ThrowsError(string addressLineValue)
        {
            var model = new Address { AddressLine = addressLineValue };
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(a => a.AddressLine);
        }

        [Fact]
        public async Task ProperStringAddressLine_NotThrowingError()
        {
            var model = new Address { AddressLine = "A Proper Address Line" };
            var result = await _validator.TestValidateAsync(model);
            result.ShouldNotHaveValidationErrorFor(a => a.AddressLine);
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        public async Task Null_Negative_OrPositiveMax_CityCode_ThrowsError(int cityCodeValue)
        {
            var model = new Address { CityCode = cityCodeValue };
            var result = await _validator.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(a => a.CityCode);
        }

        [Fact]
        public async Task ProperCityCode_NotThrowingError()
        {
            var model = new Address { CityCode = new Random().Next(0,99998)};
            var result = await _validator.TestValidateAsync(model);
            result.ShouldNotHaveValidationErrorFor(a => a.CityCode);
        }
        

    }
}