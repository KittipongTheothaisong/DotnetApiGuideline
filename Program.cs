using DotnetApiGuideline.Sources.Application;
using DotnetApiGuideline.Sources.Infrastructure.Configurations;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<AppSettings>(builder.Configuration);

builder.Services.AddConfiguredControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddConfiguredSwagger();

builder.Services.AddConfiguredAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services.AddConfiguredDatabase(builder.Configuration);
builder.Services.AddConfiguredRepositories(builder.Configuration);

builder.Services.AddConfiguredServices();

var app = builder.Build();

await app.ConfigureSeedDataAsync();

if (app.Environment.IsDevelopment())
    app.ConfigureSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
