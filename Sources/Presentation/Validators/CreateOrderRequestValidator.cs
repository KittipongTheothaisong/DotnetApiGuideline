using DotnetApiGuideline.Sources.Presentation.Requests;
using FluentValidation;

namespace DotnetApiGuideline.Sources.Presentation.Validators;

public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderRequestValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithMessage("Customer ID is required")
            .Must(BeValidGuid)
            .WithMessage("Customer ID must be a valid GUID");

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("Order must have at least one item")
            .Must(items => items.All(i => i.Quantity > 0))
            .WithMessage("All items must have quantity greater than 0")
            .Must(items => items.Count <= 50)
            .WithMessage("Order cannot have more than 50 items");

        RuleForEach(x => x.Items).SetValidator(new OrderItemRequestValidator());

        RuleFor(x => x.ShippingAddress)
            .NotNull()
            .WithMessage("Shipping address is required")
            .SetValidator(new CreateAddressRequestValidator());

        RuleFor(x => x.Notes)
            .MaximumLength(1000)
            .WithMessage("Notes cannot exceed 1000 characters");
    }

    private static bool BeValidGuid(Guid guid)
    {
        return guid != Guid.Empty;
    }
}
