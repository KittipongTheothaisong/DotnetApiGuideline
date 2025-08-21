namespace DotnetApiGuideline.Sources.Infrastructure.Configurations;

public class AppSettings
{
    public ConnectionStrings ConnectionStrings { get; set; } = new();
    public MongoDbSettings MongoDbSettings { get; set; } = new();
    public KeycloakSettings Keycloak { get; set; } = new();
    public bool UseMongoDb { get; set; } = false;
    public Logging Logging { get; set; } = new();
    public string AllowedHosts { get; set; } = "*";
}

public class ConnectionStrings
{
    public string DefaultConnection { get; set; } = string.Empty;
}

public class MongoDbSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
}

public class KeycloakSettings
{
    public string Authority { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string MetadataAddress { get; set; } = string.Empty;
}

public class Logging
{
    public LogLevel LogLevel { get; set; } = new();
}

public class LogLevel
{
    public string Default { get; set; } = "Information";
    public string MicrosoftAspNetCore { get; set; } = "Warning";
}
