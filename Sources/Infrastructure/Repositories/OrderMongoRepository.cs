using DotnetApiGuideline.Sources.Domain.Entities;
using DotnetApiGuideline.Sources.Domain.Enums;
using DotnetApiGuideline.Sources.Domain.Interfaces;
using DotnetApiGuideline.Sources.Infrastructure.Data;
using MongoDB.Driver;

namespace DotnetApiGuideline.Sources.Infrastructure.Repositories;

public class OrderMongoRepository(MongoDbContext dbContext) : IOrderRepository
{
    private readonly MongoDbContext _dbContext = dbContext;

    public async Task<OrderEntity> GetOrderByIdAsync(Guid id)
    {
        var order =
            await _dbContext.Orders.Find(o => o.Id == id).FirstOrDefaultAsync()
            ?? throw new KeyNotFoundException($"Order with ID {id} not found.");

        return order;
    }

    public async Task<OrderEntity> GetOrderByOrderNumberAsync(string orderNumber)
    {
        var order =
            await _dbContext.Orders.Find(o => o.OrderNumber == orderNumber).FirstOrDefaultAsync()
            ?? throw new KeyNotFoundException($"Order with Order Number {orderNumber} not found.");

        return order;
    }

    public async Task<IEnumerable<OrderEntity>> GetOrdersAsync()
    {
        return await _dbContext.Orders.Find(_ => true).ToListAsync();
    }

    public async Task<IEnumerable<OrderEntity>> GetOrdersByCustomerEmailAsync(string customerEmail)
    {
        var filter = Builders<OrderEntity>.Filter.Eq(o => o.Customer.Email.Value, customerEmail);
        return await _dbContext.Orders.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<OrderEntity>> GetOrdersByStatusAsync(OrderStatus status)
    {
        var filter = Builders<OrderEntity>.Filter.Eq(o => o.Status, status);
        return await _dbContext.Orders.Find(filter).ToListAsync();
    }

    public async Task<bool> OrderExistsAsync(Guid id)
    {
        var count = await _dbContext.Orders.CountDocumentsAsync(o => o.Id == id);
        return count > 0;
    }

    public async Task<bool> OrderNumberExistsAsync(string orderNumber)
    {
        var count = await _dbContext.Orders.CountDocumentsAsync(o => o.OrderNumber == orderNumber);
        return count > 0;
    }

    public async Task<OrderEntity> CreateOrderAsync(OrderEntity order)
    {
        await _dbContext.Orders.InsertOneAsync(order);
        return order;
    }

    public async Task CreateOrdersAsync(IEnumerable<OrderEntity> orders)
    {
        await _dbContext.Orders.InsertManyAsync(orders);
    }

    public async Task UpdateOrderAsync(OrderEntity order)
    {
        var result = await _dbContext.Orders.ReplaceOneAsync(o => o.Id == order.Id, order);

        if (result.MatchedCount == 0)
        {
            throw new KeyNotFoundException($"Order with ID {order.Id} not found.");
        }
    }

    public async Task DeleteOrderAsync(Guid id)
    {
        var result = await _dbContext.Orders.DeleteOneAsync(o => o.Id == id);

        if (result.DeletedCount == 0)
        {
            throw new KeyNotFoundException($"Order with ID {id} not found.");
        }
    }
}
