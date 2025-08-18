using DotnetApiGuideline.Sources.Domain.Enums;
using DotnetApiGuideline.Sources.Presentation.Requests;

namespace DotnetApiGuideline.Sources.Application.Interfaces;

public interface IOrderService
{
    Task<IEnumerable<Models.Order>> GetOrdersAsync();
    Task<Models.Order> GetOrderByIdAsync(Guid id);
    Task<Models.Order> GetOrderByOrderNumberAsync(string orderNumber);
    Task<IEnumerable<Models.Order>> GetOrdersByCustomerEmailAsync(string customerEmail);
    Task<IEnumerable<Models.Order>> GetOrdersByStatusAsync(OrderStatus status);
    Task<Models.Order> CreateOrderAsync(CreateOrderRequest createRequest);
    Task<Models.Order> UpdateOrderAsync(Guid id, UpdateOrderRequest updateRequest);
    Task<Models.Order> UpdateOrderStatusAsync(Guid id, OrderStatus status);
    Task DeleteOrderAsync(Guid id);
    Task<bool> OrderNumberExistsAsync(string orderNumber);
}
