using DotnetApiGuideline.Sources.Application.Interfaces;
using DotnetApiGuideline.Sources.Application.Services;
using DotnetApiGuideline.Sources.Domain.Interfaces;
using DotnetApiGuideline.Sources.Infrastructure.Configurations;
using DotnetApiGuideline.Sources.Infrastructure.Data;
using DotnetApiGuideline.Sources.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DotnetApiGuideline.Sources.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // Configure settings
        var settings = new AppSettings();
        configuration.Bind(settings);
        services.AddSingleton(settings);

        // Add DbContext with SQL Server using settings
        services.AddDbContext<AppDbContext>(options =>
            options
                .UseSqlServer(settings.ConnectionStrings.DefaultConnection)
                .UseSnakeCaseNamingConvention()
        );

        // Register repositories
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();

        return services;
    }

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register application services
        services.AddScoped<IOrderService, OrderService>();

        return services;
    }
}
