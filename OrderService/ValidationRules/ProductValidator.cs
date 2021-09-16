using Entities.Models;
using FluentValidation;

namespace OrderService.ValidationRules
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(c => c.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("{PropertyName} is empty.")
                .Length(2, 300).WithMessage("Length of {PropertyName} must between 2 and 300.");

            RuleFor(c => c.ImageUrl)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("{PropertyName} is empty.");


        }
        
    }
}