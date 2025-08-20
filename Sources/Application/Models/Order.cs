using DotnetApiGuideline.Sources.Domain.Enums;
using DotnetApiGuideline.Sources.Domain.ValueObjects;

namespace DotnetApiGuideline.Sources.Application.Models;

public record Order(
    Guid Id,
    string OrderNumber,
    Customer Customer,
    List<OrderItem> Items,
    Money TotalAmount,
    OrderStatus Status,
    Address ShippingAddress,
    string? Notes = null
)
{
    public static Order FromEntity(Domain.Entities.OrderEntity entity)
    {
        return new Order(
            Id: entity.Id,
            OrderNumber: entity.OrderNumber,
            Customer: Customer.FromEntity(entity.Customer),
            Items: entity.Items.Select(OrderItem.FromEntity).ToList(),
            TotalAmount: entity.TotalAmount,
            Status: entity.Status,
            ShippingAddress: entity.ShippingAddress,
            Notes: entity.Notes
        );
    }
}
