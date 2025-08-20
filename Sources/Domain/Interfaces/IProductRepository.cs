using DotnetApiGuideline.Sources.Domain.Entities;

namespace DotnetApiGuideline.Sources.Domain.Interfaces;

public interface IProductRepository
{
    Task<ProductEntity> GetProductByIdAsync(Guid id);
    Task<IEnumerable<ProductEntity>> GetProductsAsync();
    Task<IEnumerable<ProductEntity>> GetProductsByIdsAsync(IEnumerable<Guid> ids);
    Task<ProductEntity> CreateProductAsync(ProductEntity product);
    Task CreateProductsAsync(IEnumerable<ProductEntity> products);
    Task<ProductEntity> UpdateProductAsync(ProductEntity product);
    Task DeleteProductAsync(Guid id);
}
