using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DotnetApiGuideline.Sources.Infrastructure.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        // For design-time migrations, use SQL Server (this is just for schema generation)
        // The actual runtime will still use InMemory database as configured in DependencyInjection
        optionsBuilder.UseSqlServer(
            "Server=(localdb)\\mssqllocaldb;Database=DotnetApiGuideline_DesignTime;Trusted_Connection=true;TrustServerCertificate=true;"
        );

        return new AppDbContext(optionsBuilder.Options);
    }
}
