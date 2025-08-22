using DotnetApiGuideline.Sources.Presentation.Requests;
using FluentValidation;

namespace DotnetApiGuideline.Sources.Presentation.Validators;

public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderRequestValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty().WithMessage("Customer ID is required");
        RuleFor(x => x.Items).NotNull().NotEmpty().WithMessage("Order must have at least one item");

        RuleForEach(x => x.Items)
            .Must(item => item.Quantity > 0)
            .WithMessage("All items must have quantity greater than 0");

        RuleFor(x => x.ShippingAddress)
            .NotNull()
            .WithMessage("Shipping address is required")
            .SetValidator(new CreateAddressRequestValidator());
    }
}
