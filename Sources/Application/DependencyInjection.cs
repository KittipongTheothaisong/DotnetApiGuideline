using DotnetApiGuideline.Sources.Application.Interfaces;
using DotnetApiGuideline.Sources.Application.Services;

namespace DotnetApiGuideline.Sources.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IOrderService, OrderService>();

        return services;
    }
}
