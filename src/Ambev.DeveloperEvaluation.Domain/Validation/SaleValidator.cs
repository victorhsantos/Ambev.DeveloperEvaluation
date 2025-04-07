using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation
{
    public class SaleValidator : AbstractValidator<Sale>
    {
        public SaleValidator()
        {
            RuleFor(x => x.Customer)
                .NotNull().WithMessage("Customer is required.")
                .SetValidator(new CustomerValidator());

            RuleFor(x => x.PaymentAddress)
                .NotNull().WithMessage("Payment address is required.")
                .SetValidator(new PaymentAddressValidator());

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("At least one item is required.")
                .ForEach(item => item.SetValidator(new SaleItemValidator()));
        }
    }
}
