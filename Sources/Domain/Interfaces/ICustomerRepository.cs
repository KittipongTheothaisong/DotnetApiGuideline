using DotnetApiGuideline.Sources.Domain.Entities;

namespace DotnetApiGuideline.Sources.Domain.Interfaces;

public interface ICustomerRepository
{
    Task<CustomerEntity> GetCustomerByIdAsync(Guid id);
    Task<IEnumerable<CustomerEntity>> GetCustomersAsync();
    Task<CustomerEntity> CreateCustomerAsync(CustomerEntity customer);
    Task<CustomerEntity> UpdateCustomerAsync(CustomerEntity customer);
    Task DeleteCustomerAsync(string customerId);
}
