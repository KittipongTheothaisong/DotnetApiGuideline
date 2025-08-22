using DotnetApiGuideline.Sources.Presentation.Requests;
using FluentValidation;

namespace DotnetApiGuideline.Sources.Presentation.Validators;

public class CreateAddressRequestValidator : AbstractValidator<AddressRequest>
{
    public CreateAddressRequestValidator()
    {
        RuleFor(x => x.City).NotNull().NotEmpty().WithMessage("City is required");
        RuleFor(x => x.State).NotNull().NotEmpty().WithMessage("State is required");
        RuleFor(x => x.ZipCode).NotNull().NotEmpty().WithMessage("Zip code is required");
        RuleFor(x => x.Country).NotNull().NotEmpty().WithMessage("Country is required");
    }
}
