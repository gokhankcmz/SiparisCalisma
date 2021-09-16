using System;
using System.Linq;
using Entities.RequestModels;
using FluentValidation;

namespace CustomerService.ValidationRules
{
    public class UpdateCustomerDtoValidator : AbstractValidator<UpdateCustomerDto>
    {
        public UpdateCustomerDtoValidator()
        {
            
            RuleFor(c => c.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("{PropertyName} is empty.")
                .Length(2, 50).WithMessage("Length of {PropertyName} must be between 2 and 50.")
                .Must(BeAValidName).WithMessage("{PropertyName} contains invalid character(s).");

            RuleFor(c => c.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("{PropertyName} is empty.")
                .EmailAddress().WithMessage("{PropertyName} is not a valid email address.");
            
        }
        private bool BeAValidName(string name)
        {
            name = name.Replace(" ", "");
            name = name.Replace("-", "");
            return name.All(Char.IsLetter);
        }
    }
}