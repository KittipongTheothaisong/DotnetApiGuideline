using DotnetApiGuideline.Sources.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DotnetApiGuideline.Sources.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<OrderEntity> Orders { get; set; }
    public DbSet<CustomerEntity> Customers { get; set; }
    public DbSet<ProductEntity> Products { get; set; }
}
