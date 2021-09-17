using System;
using System.Data;
using System.Linq;
using Entities.RequestModels;
using FluentValidation;

namespace CustomerService.ValidationRules
{
    public class CreateCustomerDtoValidator : AbstractValidator<CreateCustomerDto>
    {
        public CreateCustomerDtoValidator()
        {

            RuleFor(c => c.Name)
                .PersonName(2, 50);
            RuleFor(c => c.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("{PropertyName} is empty.")
                .EmailAddress().WithMessage("{PropertyName} is not a valid email address.");
            
        }

    }
}