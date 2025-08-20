using DotnetApiGuideline.Sources.Domain.Entities;
using DotnetApiGuideline.Sources.Domain.Interfaces;
using DotnetApiGuideline.Sources.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DotnetApiGuideline.Sources.Infrastructure.Repositories;

public class ProductRepository(AppDbContext dbContext) : IProductRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<ProductEntity> GetProductByIdAsync(Guid id)
    {
        var product =
            await _dbContext.Products.FindAsync(id)
            ?? throw new KeyNotFoundException($"Product with ID {id} not found.");

        return product;
    }

    public async Task<IEnumerable<ProductEntity>> GetProductsAsync()
    {
        return await _dbContext.Products.ToListAsync();
    }

    public async Task<IEnumerable<ProductEntity>> GetProductsByIdsAsync(IEnumerable<Guid> ids)
    {
        return await _dbContext.Products.Where(p => ids.Contains(p.Id)).ToListAsync();
    }

    public async Task<ProductEntity> CreateProductAsync(ProductEntity Product)
    {
        _dbContext.Products.Add(Product);
        await _dbContext.SaveChangesAsync();
        return Product;
    }

    public Task CreateProductsAsync(IEnumerable<ProductEntity> products)
    {
        _dbContext.Products.AddRange(products);
        return _dbContext.SaveChangesAsync();
    }

    public async Task DeleteProductAsync(Guid id)
    {
        var product = await GetProductByIdAsync(id);

        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<ProductEntity> UpdateProductAsync(ProductEntity Product)
    {
        var existingProduct = await GetProductByIdAsync(Product.Id);

        _dbContext.Products.Entry(existingProduct).CurrentValues.SetValues(Product);

        await _dbContext.SaveChangesAsync();
        return existingProduct;
    }
}
