using DotnetApiGuideline.Sources.Presentation.Requests;
using FluentValidation;

namespace DotnetApiGuideline.Sources.Presentation.Validators;

public class OrderItemRequestValidator : AbstractValidator<OrderItemRequest>
{
    public OrderItemRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required")
            .Must(BeValidGuid)
            .WithMessage("Product ID must be a valid GUID");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0")
            .LessThanOrEqualTo(1000)
            .WithMessage("Quantity cannot exceed 1000 units per item");
    }

    private static bool BeValidGuid(Guid guid)
    {
        return guid != Guid.Empty;
    }
}
