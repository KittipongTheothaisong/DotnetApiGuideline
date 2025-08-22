using DotnetApiGuideline.Sources.Domain.Interfaces;
using DotnetApiGuideline.Sources.Infrastructure.Repositories;

namespace DotnetApiGuideline.Sources.Infrastructure.Configurations;

public static class RepositoriesConfiguration
{
    public static IServiceCollection AddConfiguredRepositories(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var settings =
            configuration.Get<AppSettings>() ?? throw new Exception("App settings not found");

        if (settings.UseMongoDb)
        {
            services.AddMongoRepositories();
        }
        else
        {
            services.AddSqlRepositories();
        }

        return services;
    }

    private static IServiceCollection AddMongoRepositories(this IServiceCollection services)
    {
        services.AddScoped<IProductRepository, ProductMongoRepository>();
        services.AddScoped<IOrderRepository, OrderMongoRepository>();
        services.AddScoped<ICustomerRepository, CustomerMongoRepository>();

        return services;
    }

    private static IServiceCollection AddSqlRepositories(this IServiceCollection services)
    {
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();

        return services;
    }
}
