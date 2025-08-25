using DotnetApiGuideline.Sources.Presentation.Requests;
using FluentValidation;

namespace DotnetApiGuideline.Sources.Presentation.Validators;

public class UpdateOrderRequestValidator : AbstractValidator<UpdateOrderRequest>
{
    public UpdateOrderRequestValidator()
    {
        When(
            x => x.Items != null,
            () =>
            {
                RuleFor(x => x.Items)
                    .Must(items => (items ?? []).Count > 0)
                    .WithMessage("If items are provided, there must be at least one item")
                    .Must(items => (items ?? []).Count <= 50)
                    .WithMessage("Order cannot have more than 50 items");

                RuleForEach(x => x.Items).SetValidator(new OrderItemRequestValidator());
            }
        );

        When(
            x => x.ShippingAddress != null,
            () =>
            {
                RuleFor(x => x.ShippingAddress).SetValidator(new CreateAddressRequestValidator()!);
            }
        );

        When(
            x => x.Notes != null,
            () =>
            {
                RuleFor(x => x.Notes)
                    .MaximumLength(1000)
                    .WithMessage("Notes cannot exceed 1000 characters");
            }
        );
    }
}
