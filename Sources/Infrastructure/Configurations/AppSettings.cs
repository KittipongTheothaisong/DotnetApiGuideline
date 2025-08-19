namespace DotnetApiGuideline.Sources.Infrastructure.Configurations;

public class AppSettings
{
    public ConnectionStrings ConnectionStrings { get; set; } = new();
    public Logging Logging { get; set; } = new();
    public string AllowedHosts { get; set; } = "*";
}

public class ConnectionStrings
{
    public string DefaultConnection { get; set; } = string.Empty;
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
