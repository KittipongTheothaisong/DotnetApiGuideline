namespace DotnetApiGuideline.Sources.Presentation.Requests;

public record UpdateOrderRequest(
    List<OrderItemRequest>? Items,
    AddressRequest? ShippingAddress,
    string? Notes = null
);
