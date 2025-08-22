using Microsoft.OpenApi.Models;

namespace DotnetApiGuideline.Sources.Infrastructure.Configurations;

public static class SwaggerConfiguration
{
    private const string ApiVersion = "v1";
    private const string ApiTitle = "DotnetApiGuideline API";
    private const string ApiDescription = "A sample API following .NET API guidelines";
    private const string SecurityScheme = "Bearer";
    private const string AuthorizationHeader = "Authorization";
    private const string JwtFormat = "JWT";
    private const string SecuritySchemeDescription =
        "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"";
    private const string SwaggerEndpointUrl = "/swagger/v1/swagger.json";
    private const string SwaggerEndpointName = "DotnetApiGuideline API v1";

    public static IServiceCollection AddConfiguredSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(
                ApiVersion,
                new OpenApiInfo
                {
                    Title = ApiTitle,
                    Version = ApiVersion,
                    Description = ApiDescription,
                }
            );

            c.AddSecurityDefinition(
                SecurityScheme,
                new OpenApiSecurityScheme
                {
                    Description = SecuritySchemeDescription,
                    Name = AuthorizationHeader,
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = SecurityScheme,
                    BearerFormat = JwtFormat,
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
                                Id = SecurityScheme,
                            },
                        },
                        Array.Empty<string>()
                    },
                }
            );
        });

        return services;
    }

    public static void ConfigureSwaggerUI(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint(SwaggerEndpointUrl, SwaggerEndpointName);
            c.RoutePrefix = string.Empty;
        });
    }
}
