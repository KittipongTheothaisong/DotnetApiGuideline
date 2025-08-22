using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace DotnetApiGuideline.Sources.Infrastructure.Configurations;

public static class AuthenticationConfiguration
{
    public static IServiceCollection AddConfiguredAuthentication(
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
