namespace OrderManagement.Domain.ValueObjects;

public class Address(string street, string city, string state, string country, string zipCode)
    : IEquatable<Address>
{
    public string Street { get; } = street ?? throw new ArgumentNullException(nameof(street));
    public string City { get; } = city ?? throw new ArgumentNullException(nameof(city));
    public string State { get; } = state ?? throw new ArgumentNullException(nameof(state));
    public string Country { get; } = country ?? throw new ArgumentNullException(nameof(country));
    public string ZipCode { get; } = zipCode ?? throw new ArgumentNullException(nameof(zipCode));

    public static Address Empty() =>
        new(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

    public bool Equals(Address? other)
    {
        if (other is null)
            return false;
        return Street == other.Street
            && City == other.City
            && State == other.State
            && Country == other.Country
            && ZipCode == other.ZipCode;
    }

    public override bool Equals(object? obj) => Equals(obj as Address);

    public override int GetHashCode() => HashCode.Combine(Street, City, State, Country, ZipCode);

    public override string ToString() => $"{Street}, {City}, {State} {ZipCode}, {Country}";
}
