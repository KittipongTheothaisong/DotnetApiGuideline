using DotnetApiGuideline.Sources.Domain.ValueObjects;

namespace DotnetApiGuideline.Sources.Presentation.Responses;

public record AddressResponse(
    string Street,
    string City,
    string State,
    string ZipCode,
    string Country
)
{
    public static AddressResponse FromAddress(Address address)
    {
        return new AddressResponse(
            Street: address.Street,
            City: address.City,
            State: address.State,
            ZipCode: address.ZipCode,
            Country: address.Country
        );
    }
}
