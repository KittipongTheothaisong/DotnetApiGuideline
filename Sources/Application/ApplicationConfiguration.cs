using DotnetApiGuideline.Sources.Application.Interfaces;
using DotnetApiGuideline.Sources.Application.Services;

namespace DotnetApiGuideline.Sources.Application;

public static class ApplicationConfiguration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IOrderService, OrderService>();

        return services;
    }
}
