using System.ComponentModel.DataAnnotations.Schema;
using DotnetApiGuideline.Sources.Domain.ValueObjects;

namespace DotnetApiGuideline.Sources.Domain.Entities;

[Table("products")]
public class ProductEntity : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string Sku { get; private set; } = string.Empty;
    public Money Price { get; private set; } = Money.Zero();
    public int StockQuantity { get; private set; }
    public bool IsActive { get; private set; }

    protected ProductEntity() { }

    public ProductEntity(
        string name,
        string description,
        string sku,
        Money price,
        int stockQuantity,
        bool isActive = true
    )
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Name cannot be null or empty", nameof(name));
        if (string.IsNullOrEmpty(description))
            throw new ArgumentException("Description cannot be null or empty", nameof(description));
        if (string.IsNullOrEmpty(sku))
            throw new ArgumentException("SKU cannot be null or empty", nameof(sku));
        if (stockQuantity < 0)
            throw new ArgumentException("Stock quantity cannot be negative", nameof(stockQuantity));

        Name = name;
        Description = description;
        Sku = sku;
        Price = price ?? throw new ArgumentNullException(nameof(price), "Price cannot be null");
        StockQuantity = stockQuantity;
        IsActive = isActive;
    }
}
