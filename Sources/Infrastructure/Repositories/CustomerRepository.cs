using DotnetApiGuideline.Sources.Domain.Entities;
using DotnetApiGuideline.Sources.Domain.Interfaces;
using DotnetApiGuideline.Sources.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DotnetApiGuideline.Sources.Infrastructure.Repositories;

public class CustomerRepository(AppDbContext dbContext) : ICustomerRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<CustomerEntity> GetCustomerByIdAsync(Guid id)
    {
        return await _dbContext.Customers.FindAsync(id)
            ?? throw new KeyNotFoundException($"Customer with ID {id} not found.");
    }

    public async Task<IEnumerable<CustomerEntity>> GetCustomersAsync()
    {
        return await _dbContext.Customers.ToListAsync();
    }

    public async Task<CustomerEntity> CreateCustomerAsync(CustomerEntity customer)
    {
        _dbContext.Customers.Add(customer);
        await _dbContext.SaveChangesAsync();

        return customer;
    }

    public async Task<CustomerEntity> UpdateCustomerAsync(CustomerEntity customer)
    {
        var existingCustomer = await GetCustomerByIdAsync(customer.Id);

        _dbContext.Customers.Entry(existingCustomer).CurrentValues.SetValues(customer);
        await _dbContext.SaveChangesAsync();

        return existingCustomer;
    }

    public async Task DeleteCustomerAsync(string customerId)
    {
        var customer =
            await GetCustomerByIdAsync(Guid.Parse(customerId))
            ?? throw new KeyNotFoundException($"Customer with ID {customerId} not found.");

        _dbContext.Customers.Remove(customer);

        await _dbContext.SaveChangesAsync();
    }
}
