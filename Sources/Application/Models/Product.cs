using DotnetApiGuideline.Sources.Domain.Entities;
using DotnetApiGuideline.Sources.Domain.ValueObjects;

namespace DotnetApiGuideline.Sources.Application.Models;

public record Product(
    Guid Id,
    string Name,
    string Description,
    string Sku,
    Money Price,
    int StockQuantity,
    bool IsActive
)
{
    public static Product FromEntity(ProductEntity entity)
    {
        return new Product(
            Id: entity.Id,
            Name: entity.Name,
            Description: entity.Description,
            Sku: entity.Sku,
            Price: entity.Price,
            StockQuantity: entity.StockQuantity,
            IsActive: entity.IsActive
        );
    }
}
