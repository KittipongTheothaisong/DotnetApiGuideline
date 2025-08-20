using DotnetApiGuideline.Sources.Domain.Entities;
using DotnetApiGuideline.Sources.Domain.Interfaces;
using DotnetApiGuideline.Sources.Infrastructure.Data;
using MongoDB.Driver;

namespace DotnetApiGuideline.Sources.Infrastructure.Repositories;

public class CustomerMongoRepository(MongoDbContext dbContext) : ICustomerRepository
{
    private readonly MongoDbContext _dbContext = dbContext;

    public async Task<CustomerEntity> GetCustomerByIdAsync(Guid id)
    {
        var customer =
            await _dbContext.Customers.Find(c => c.Id == id).FirstOrDefaultAsync()
            ?? throw new KeyNotFoundException($"Customer with ID {id} not found.");

        return customer;
    }

    public async Task<IEnumerable<CustomerEntity>> GetCustomersAsync()
    {
        return await _dbContext.Customers.Find(_ => true).ToListAsync();
    }

    public async Task<CustomerEntity> CreateCustomerAsync(CustomerEntity customer)
    {
        await _dbContext.Customers.InsertOneAsync(customer);
        return customer;
    }

    public Task CreateCustomersAsync(IEnumerable<CustomerEntity> customers)
    {
        return _dbContext.Customers.InsertManyAsync(customers);
    }

    public async Task<CustomerEntity> UpdateCustomerAsync(CustomerEntity customer)
    {
        var result = await _dbContext.Customers.ReplaceOneAsync(c => c.Id == customer.Id, customer);

        if (result.MatchedCount == 0)
        {
            throw new KeyNotFoundException($"Customer with ID {customer.Id} not found.");
        }

        return customer;
    }

    public async Task DeleteCustomerAsync(Guid id)
    {
        var result = await _dbContext.Customers.DeleteOneAsync(c => c.Id == id);

        if (result.DeletedCount == 0)
        {
            throw new KeyNotFoundException($"Customer with ID {id} not found.");
        }
    }
}
