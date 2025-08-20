using DotnetApiGuideline.Sources.Domain.Entities;
using DotnetApiGuideline.Sources.Domain.Interfaces;
using DotnetApiGuideline.Sources.Domain.ValueObjects;

namespace DotnetApiGuideline.Sources.Infrastructure;

public class DataInitializer
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();

        await CleanupAsync(serviceProvider);
        await SeedProductsAsync(productRepository);
    }

    private static async Task CleanupAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();

        await DeleteAllProductsAsync(productRepository);
    }

    private static async Task DeleteAllProductsAsync(IProductRepository productRepository)
    {
        var products = await productRepository.GetProductsAsync();
        foreach (var product in products)
        {
            await productRepository.DeleteProductAsync(product.Id);
        }
    }

    private static async Task SeedProductsAsync(IProductRepository productRepository)
    {
        var products = new List<ProductEntity>
        {
            new(
                name: "iPhone 15 Pro",
                description: "Latest iPhone with Pro features",
                sku: "IP15P-001",
                price: new Money(39900, "THB"),
                stockQuantity: 50
            ),
            new(
                name: "MacBook Air M3",
                description: "Lightweight laptop with M3 chip",
                sku: "MBA-M3-001",
                price: new Money(42900, "THB"),
                stockQuantity: 25
            ),
            new(
                name: "AirPods Pro",
                description: "Wireless earbuds with noise cancellation",
                sku: "APP-001",
                price: new Money(8990, "THB"),
                stockQuantity: 100
            ),
            new(
                name: "iPad Air",
                description: "Powerful tablet for work and play",
                sku: "IPA-001",
                price: new Money(21900, "THB"),
                stockQuantity: 75
            ),
            new(
                name: "Apple Watch Series 9",
                description: "Advanced health and fitness tracking",
                sku: "AWS9-001",
                price: new Money(13900, "THB"),
                stockQuantity: 40
            ),
        };

        await productRepository.CreateProductsAsync(products);
    }
}
