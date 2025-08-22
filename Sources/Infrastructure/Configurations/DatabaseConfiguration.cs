using DotnetApiGuideline.Sources.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;

namespace DotnetApiGuideline.Sources.Infrastructure.Configurations;

public static class DatabaseConfiguration
{
    public static IServiceCollection AddConfiguredDatabase(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var settings =
            configuration.Get<AppSettings>() ?? throw new Exception("App settings not found");

        if (settings.UseMongoDb)
        {
            AddMongoDb(services, settings);
        }
        else
        {
            AddSqlDb(services, settings);
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
}
