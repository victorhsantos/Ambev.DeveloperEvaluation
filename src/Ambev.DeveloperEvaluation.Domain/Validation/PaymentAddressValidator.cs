using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation
{
    public class PaymentAddressValidator : AbstractValidator<PaymentAddress>
    {
        public PaymentAddressValidator()
        {
            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("Street is required.")
                .Length(5, 100).WithMessage("Street must be between 5 and 100 characters.");
            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required.")
                .Length(2, 50).WithMessage("City must be between 2 and 50 characters.");
            RuleFor(x => x.State)
                .NotEmpty().WithMessage("State is required.")
                .Length(2, 50).WithMessage("State must be between 2 and 50 characters.");
        }
    }
}
