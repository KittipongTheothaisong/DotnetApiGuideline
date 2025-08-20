using DotnetApiGuideline.Sources.Domain.ValueObjects;

namespace DotnetApiGuideline.Sources.Presentation.Requests;

public record CreateOrderRequest(
    Guid CustomerId,
    List<OrderItemRequest> Items,
    Address ShippingAddress,
    string? Notes = null
);
