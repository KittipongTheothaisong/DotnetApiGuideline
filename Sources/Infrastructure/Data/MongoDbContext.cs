using DotnetApiGuideline.Sources.Domain.Entities;
using DotnetApiGuideline.Sources.Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DotnetApiGuideline.Sources.Infrastructure.Data;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(MongoDbSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        _database = client.GetDatabase(settings.DatabaseName);
    }

    public IMongoCollection<ProductEntity> Products =>
        _database.GetCollection<ProductEntity>("products");

    public IMongoCollection<CustomerEntity> Customers =>
        _database.GetCollection<CustomerEntity>("customers");

    public IMongoCollection<OrderEntity> Orders => _database.GetCollection<OrderEntity>("orders");
}
