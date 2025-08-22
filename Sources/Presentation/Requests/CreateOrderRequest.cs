namespace DotnetApiGuideline.Sources.Presentation.Requests;

public record CreateOrderRequest(
    Guid CustomerId,
    List<OrderItemRequest> Items,
    AddressRequest ShippingAddress,
    string? Notes = null
);
