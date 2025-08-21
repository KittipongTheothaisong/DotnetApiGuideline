using DotnetApiGuideline.Sources.Infrastructure;

namespace DotnetApiGuideline.Sources.Presentation.Extensions;

public static class WebApplicationExtensions
{
    public static async Task InitializeDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        await DataInitializer.InitializeAsync(scope.ServiceProvider);
    }

    public static void ConfigureSwaggerUI(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "DotnetApiGuideline API v1");
            c.RoutePrefix = string.Empty;
        });
    }
}
