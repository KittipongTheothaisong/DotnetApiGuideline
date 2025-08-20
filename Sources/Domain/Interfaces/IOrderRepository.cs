using DotnetApiGuideline.Sources.Domain.Entities;
using DotnetApiGuideline.Sources.Domain.Enums;

namespace DotnetApiGuideline.Sources.Domain.Interfaces;

public interface IOrderRepository
{
    Task<IEnumerable<OrderEntity>> GetOrdersAsync();
    Task<OrderEntity> GetOrderByIdAsync(Guid id);
    Task<OrderEntity> GetOrderByOrderNumberAsync(string orderNumber);
    Task<IEnumerable<OrderEntity>> GetOrdersByCustomerEmailAsync(string customerEmail);
    Task<IEnumerable<OrderEntity>> GetOrdersByStatusAsync(OrderStatus status);
    Task<bool> OrderExistsAsync(Guid id);
    Task<bool> OrderNumberExistsAsync(string orderNumber);
    Task<OrderEntity> CreateOrderAsync(OrderEntity order);
    Task CreateOrdersAsync(IEnumerable<OrderEntity> orders);
    Task UpdateOrderAsync(OrderEntity order);
    Task DeleteOrderAsync(Guid id);
}
