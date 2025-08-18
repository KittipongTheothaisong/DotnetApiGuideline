namespace DotnetApiGuideline.Sources.Domain.Enums;

public enum OrderStatus
{
    Unknown = -1,
    Pending = 0,
    Confirmed = 1,
    Shipped = 2,
    Delivered = 3,
    Cancelled = 4,
}
