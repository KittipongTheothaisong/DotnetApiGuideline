using System.Text.Json;

namespace DotnetApiGuideline.Sources.Infrastructure.Configurations;

public static class ControllersConfiguration
{
    public static IServiceCollection AddConfiguredControllers(this IServiceCollection services)
    {
        services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy =
                    JsonNamingPolicy.SnakeCaseLower;
                options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower;
                options.JsonSerializerOptions.WriteIndented = true;
            });

        return services;
    }
}
