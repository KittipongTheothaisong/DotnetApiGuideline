using DotnetApiGuideline.Sources.Domain.Entities;
using DotnetApiGuideline.Sources.Domain.ValueObjects;

namespace DotnetApiGuideline.Sources.Application.Models;

public record OrderItem(
    Guid Id,
    Guid OrderId,
    Product Product,
    int Quantity,
    Money UnitPrice,
    Money TotalPrice
)
{
    public static OrderItem FromEntity(OrderItemEntity entity)
    {
        return new OrderItem(
            Id: entity.Id,
            OrderId: entity.OrderId,
            Product: Product.FromEntity(entity.Product),
            Quantity: entity.Quantity,
            UnitPrice: entity.UnitPrice,
            TotalPrice: entity.TotalPrice
        );
    }
}
