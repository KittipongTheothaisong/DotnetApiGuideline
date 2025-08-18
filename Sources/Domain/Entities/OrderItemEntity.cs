using DotnetApiGuideline.Sources.Domain.ValueObjects;

namespace DotnetApiGuideline.Sources.Domain.Entities;

public class OrderItemEntity : BaseEntity
{
    public Guid OrderId { get; private set; }
    public ProductEntity Product { get; private set; } = new();
    public int Quantity { get; private set; }
    public Money UnitPrice { get; set; } = Money.Zero();
    public Money TotalPrice => UnitPrice * Quantity;

    public static OrderItemEntity FromProduct(ProductEntity product, int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(quantity),
                "Quantity must be greater than zero."
            );

        return new OrderItemEntity
        {
            OrderId = Guid.NewGuid(),
            Product = product,
            Quantity = quantity,
            UnitPrice = product.Price,
        };
    }
}
