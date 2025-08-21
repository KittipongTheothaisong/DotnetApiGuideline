using DotnetApiGuideline.Sources.Domain.Interfaces;
using DotnetApiGuideline.Sources.Infrastructure.Configurations;
using DotnetApiGuideline.Sources.Infrastructure.Data;
using DotnetApiGuideline.Sources.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;

namespace DotnetApiGuideline.Sources.Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
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

    public static IServiceCollection AddKeycloakAuthentication(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var settings =
            configuration.Get<AppSettings>()
            ?? throw new InvalidOperationException("AppSettings not configured");

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = settings.Keycloak.Authority;
                options.Audience = settings.Keycloak.Audience;
                options.IncludeErrorDetails = true;
                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidAudience = "account",
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.FromMinutes(5),
                    RoleClaimType = "realm_access.roles",
                };
            });

        return services;
    }
}
