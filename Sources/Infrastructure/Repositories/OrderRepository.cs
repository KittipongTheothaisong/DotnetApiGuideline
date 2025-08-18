using DotnetApiGuideline.Sources.Domain.Entities;
using DotnetApiGuideline.Sources.Domain.Enums;
using DotnetApiGuideline.Sources.Domain.Interfaces;
using DotnetApiGuideline.Sources.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DotnetApiGuideline.Sources.Infrastructure.Repositories;

public class OrderRepository(AppDbContext dbContext) : IOrderRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<OrderEntity> GetOrderByIdAsync(Guid id)
    {
        return await _dbContext.Orders.FindAsync(id)
            ?? throw new KeyNotFoundException($"Order with ID {id} not found.");
    }

    public async Task<OrderEntity> GetOrderByOrderNumberAsync(string orderNumber)
    {
        return await _dbContext.Orders.FirstOrDefaultAsync(o => o.OrderNumber == orderNumber)
            ?? throw new KeyNotFoundException($"Order with Order Number {orderNumber} not found.");
    }

    public async Task<IEnumerable<OrderEntity>> GetOrdersAsync()
    {
        return await _dbContext.Orders.ToListAsync();
    }

    public async Task<IEnumerable<OrderEntity>> GetOrdersByCustomerEmailAsync(string customerEmail)
    {
        return await _dbContext
            .Orders.Where(order => order.Customer.Email == customerEmail)
            .ToListAsync();
    }

    public async Task<IEnumerable<OrderEntity>> GetOrdersByStatusAsync(OrderStatus status)
    {
        return await _dbContext.Orders.Where(order => order.Status == status).ToListAsync();
    }

    public async Task<bool> OrderExistsAsync(Guid id)
    {
        return await _dbContext.Orders.AnyAsync(o => o.Id == id);
    }

    public async Task<bool> OrderNumberExistsAsync(string orderNumber)
    {
        return await _dbContext.Orders.AnyAsync(o => o.OrderNumber == orderNumber);
    }

    public async Task<OrderEntity> CreateOrderAsync(OrderEntity order)
    {
        _dbContext.Orders.Add(order);
        await _dbContext.SaveChangesAsync();
        return order;
    }

    public async Task UpdateOrderAsync(OrderEntity order)
    {
        var existingOrder = await GetOrderByIdAsync(order.Id);
        _dbContext.Orders.Entry(existingOrder).CurrentValues.SetValues(order);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteOrderAsync(Guid id)
    {
        var order = await GetOrderByIdAsync(id);
        _dbContext.Orders.Remove(order);

        await _dbContext.SaveChangesAsync();
    }
}
