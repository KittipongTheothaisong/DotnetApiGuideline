using System.Text.RegularExpressions;
using DotnetApiGuideline.Sources.Application.Interfaces;
using DotnetApiGuideline.Sources.Application.Services;
using DotnetApiGuideline.Sources.Domain.Interfaces;
using DotnetApiGuideline.Sources.Infrastructure.Configurations;
using DotnetApiGuideline.Sources.Infrastructure.Data;
using DotnetApiGuideline.Sources.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;

namespace DotnetApiGuideline.Sources.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<AppSettings>(configuration);
        var settings =
            configuration.Get<AppSettings>() ?? throw new Exception("App settings not found");

        if (settings.UseMongoDb)
        {
            AddMongoDb(services, settings);

            services.AddScoped<IProductRepository, ProductMongoRepository>();
            services.AddScoped<IOrderRepository, OrderMongoRepository>();
            services.AddScoped<ICustomerRepository, CustomerMongoRepository>();
        }
        else
        {
            AddSqlDb(services, settings);

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
        }

        return services;
    }

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IOrderService, OrderService>();

        return services;
    }

    private static void AddSqlDb(IServiceCollection services, AppSettings settings)
    {
        services.AddDbContext<AppDbContext>(options =>
            options
                .UseSqlServer(settings.ConnectionStrings.DefaultConnection)
                .UseSnakeCaseNamingConvention()
        );
    }

    private static void AddMongoDb(IServiceCollection services, AppSettings settings)
    {
        services.AddSingleton(settings.MongoDbSettings);
        services.AddSingleton<MongoDbContext>();

        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

        var conventionPack = new ConventionPack { new CamelCaseElementNameConvention() };

        ConventionRegistry.Register("DefaultConventions", conventionPack, t => true);
    }
}
