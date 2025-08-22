namespace DotnetApiGuideline.Sources.Presentation.Requests;

public record AddressRequest(
    string Street,
    string City,
    string State,
    string ZipCode,
    string Country
);
