using DotnetApiGuideline.Sources.Presentation.Requests;
using FluentValidation;

namespace DotnetApiGuideline.Sources.Presentation.Validators;

public class CreateAddressRequestValidator : AbstractValidator<AddressRequest>
{
    public CreateAddressRequestValidator()
    {
        RuleFor(x => x.Street)
            .NotEmpty()
            .WithMessage("Street address is required")
            .MaximumLength(200)
            .WithMessage("Street address cannot exceed 200 characters");

        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage("City is required")
            .MaximumLength(100)
            .WithMessage("City cannot exceed 100 characters");

        RuleFor(x => x.State)
            .NotEmpty()
            .WithMessage("State is required")
            .MaximumLength(50)
            .WithMessage("State cannot exceed 50 characters");

        RuleFor(x => x.ZipCode)
            .NotEmpty()
            .WithMessage("Zip code is required")
            .Matches(@"^\d{5}(-\d{4})?$")
            .WithMessage("Zip code must be in format 12345 or 12345-6789");

        RuleFor(x => x.Country).NotEmpty().WithMessage("Country is required");
    }
}
