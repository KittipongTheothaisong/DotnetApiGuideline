using DotnetApiGuideline.Sources.Application.Models;
using DotnetApiGuideline.Sources.Domain.Enums;

namespace DotnetApiGuideline.Sources.Presentation.Responses;

public record OrderResponse(
    Guid Id,
    string OrderNumber,
    CustomerResponse Customer,
    List<OrderItemResponse> Items,
    AddressResponse ShippingAddress,
    OrderStatus Status,
    string? Notes = null
)
{
    public static OrderResponse FromOrder(Order order)
    {
        return new OrderResponse(
            Id: order.Id,
            OrderNumber: order.OrderNumber,
            Customer: CustomerResponse.FromCustomer(order.Customer),
            Items: order.Items.Select(OrderItemResponse.FromOrderItem).ToList(),
            ShippingAddress: AddressResponse.FromAddress(order.ShippingAddress),
            Status: order.Status,
            Notes: order.Notes
        );
    }
}
