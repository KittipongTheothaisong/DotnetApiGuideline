using DotnetApiGuideline.Sources.Domain.Entities;
using DotnetApiGuideline.Sources.Domain.Interfaces;
using DotnetApiGuideline.Sources.Infrastructure.Data;
using MongoDB.Driver;

namespace DotnetApiGuideline.Sources.Infrastructure.Repositories;

public class ProductMongoRepository(MongoDbContext dbContext) : IProductRepository
{
    private readonly MongoDbContext _dbContext = dbContext;

    public async Task<ProductEntity> GetProductByIdAsync(Guid id)
    {
        var product =
            await _dbContext.Products.Find(p => p.Id == id).FirstOrDefaultAsync()
            ?? throw new KeyNotFoundException($"Product with ID {id} not found.");

        return product;
    }

    public async Task<IEnumerable<ProductEntity>> GetProductsAsync()
    {
        return await _dbContext.Products.Find(_ => true).ToListAsync();
    }

    public async Task<IEnumerable<ProductEntity>> GetProductsByIdsAsync(IEnumerable<Guid> ids)
    {
        var filter = Builders<ProductEntity>.Filter.In(p => p.Id, ids);
        return await _dbContext.Products.Find(filter).ToListAsync();
    }

    public async Task<ProductEntity> CreateProductAsync(ProductEntity product)
    {
        await _dbContext.Products.InsertOneAsync(product);
        return product;
    }

    public async Task CreateProductsAsync(IEnumerable<ProductEntity> products)
    {
        await _dbContext.Products.InsertManyAsync(products);
    }

    public async Task DeleteProductAsync(Guid id)
    {
        var result = await _dbContext.Products.DeleteOneAsync(p => p.Id == id);

        if (result.DeletedCount == 0)
        {
            throw new KeyNotFoundException($"Product with ID {id} not found.");
        }
    }

    public async Task<ProductEntity> UpdateProductAsync(ProductEntity product)
    {
        var result = await _dbContext.Products.ReplaceOneAsync(p => p.Id == product.Id, product);

        if (result.MatchedCount == 0)
        {
            throw new KeyNotFoundException($"Product with ID {product.Id} not found.");
        }

        return product;
    }
}
