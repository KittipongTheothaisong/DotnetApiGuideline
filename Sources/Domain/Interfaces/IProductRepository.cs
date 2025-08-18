using DotnetApiGuideline.Sources.Domain.Entities;

namespace DotnetApiGuideline.Sources.Domain.Interfaces;

public interface IProductRepository
{
    Task<ProductEntity> GetProductByIdAsync(Guid id);
    Task<IEnumerable<ProductEntity>> GetProductsAsync();
    Task<IEnumerable<ProductEntity>> GetProductsByIdsAsync(IEnumerable<Guid> ids);
    Task<ProductEntity> CreateProductAsync(ProductEntity Product);
    Task<ProductEntity> UpdateProductAsync(ProductEntity Product);
    Task DeleteProductAsync(Guid id);
}
