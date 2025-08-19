using DotnetApiGuideline.Sources.Domain.Entities;
using DotnetApiGuideline.Sources.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace DotnetApiGuideline.Sources.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<OrderEntity> Orders { get; set; }
    public DbSet<CustomerEntity> Customers { get; set; }
    public DbSet<ProductEntity> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CustomerEntity>(entity =>
        {
            entity
                .Property(e => e.Email)
                .HasConversion(
                    email => email.Value,
                    value => string.IsNullOrEmpty(value) ? Email.Empty() : new Email(value)
                )
                .HasColumnName("email");

            entity.OwnsOne(
                e => e.Address,
                address =>
                {
                    address.Property(a => a.Street).HasColumnName("address_street");
                    address.Property(a => a.City).HasColumnName("address_city");
                    address.Property(a => a.State).HasColumnName("address_state");
                    address.Property(a => a.Country).HasColumnName("address_country");
                    address.Property(a => a.ZipCode).HasColumnName("address_zip_code");
                }
            );
        });

        modelBuilder.Entity<OrderEntity>(entity =>
        {
            entity.OwnsOne(
                e => e.ShippingAddress,
                address =>
                {
                    address.Property(a => a.Street).HasColumnName("shipping_address_street");
                    address.Property(a => a.City).HasColumnName("shipping_address_city");
                    address.Property(a => a.State).HasColumnName("shipping_address_state");
                    address.Property(a => a.Country).HasColumnName("shipping_address_country");
                    address.Property(a => a.ZipCode).HasColumnName("shipping_address_zip_code");
                }
            );

            entity.HasOne(e => e.Customer).WithMany().HasForeignKey("CustomerId");

            entity.HasMany(e => e.Items).WithOne().HasForeignKey(oi => oi.OrderId);
        });

        modelBuilder.Entity<ProductEntity>(entity =>
        {
            entity.OwnsOne(
                e => e.Price,
                money =>
                {
                    money
                        .Property(m => m.Amount)
                        .HasColumnName("price_amount")
                        .HasColumnType("decimal(18,2)");
                    money.Property(m => m.Currency).HasColumnName("price_currency").HasMaxLength(3);
                }
            );
        });

        modelBuilder.Entity<OrderItemEntity>(entity =>
        {
            entity.OwnsOne(
                e => e.UnitPrice,
                money =>
                {
                    money
                        .Property(m => m.Amount)
                        .HasColumnName("unit_price_amount")
                        .HasColumnType("decimal(18,2)");
                    money
                        .Property(m => m.Currency)
                        .HasColumnName("unit_price_currency")
                        .HasMaxLength(3);
                }
            );

            entity.HasOne(e => e.Product).WithMany().HasForeignKey("ProductId");

            entity.Ignore(e => e.TotalPrice);
        });
    }
}
