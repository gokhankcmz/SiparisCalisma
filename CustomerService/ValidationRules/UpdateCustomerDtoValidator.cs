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

            RuleFor(c => c.Name).PersonName(2, 50);
            RuleFor(c => c.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("{PropertyName} is empty.")
                .EmailAddress().WithMessage("{PropertyName} is not a valid email address.");
            
        }
    }
}