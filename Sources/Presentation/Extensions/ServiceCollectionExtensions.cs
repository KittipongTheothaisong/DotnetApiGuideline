using System.Text.Json;
using DotnetApiGuideline.Sources.Infrastructure.Configurations;
using Microsoft.OpenApi.Models;

namespace DotnetApiGuideline.Sources.Presentation.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSettings(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<AppSettings>(configuration);
        return services;
    }

    public static IServiceCollection ConfigureControllers(this IServiceCollection services)
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

    public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(
                "v1",
                new OpenApiInfo
                {
                    Title = "DotnetApiGuideline API",
                    Version = "v1",
                    Description = "A sample API following .NET API guidelines",
                }
            );

            c.AddSecurityDefinition(
                "Bearer",
                new OpenApiSecurityScheme
                {
                    Description =
                        "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                }
            );

            c.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                        },
                        Array.Empty<string>()
                    },
                }
            );
        });

        return services;
    }
}
