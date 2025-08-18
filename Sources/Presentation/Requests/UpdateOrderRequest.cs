using OrderManagement.Domain.ValueObjects;

namespace DotnetApiGuideline.Sources.Presentation.Requests;

public record UpdateOrderRequest(
    List<OrderItemRequest>? Items,
    Address? ShippingAddress,
    string? Notes = null
);
