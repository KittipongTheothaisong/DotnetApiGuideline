using DotnetApiGuideline.Sources.Application;
using DotnetApiGuideline.Sources.Infrastructure;
using DotnetApiGuideline.Sources.Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSettings(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureControllers();
builder.Services.ConfigureSwagger();

builder.Services.AddKeycloakAuthentication(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddDatabase(builder.Configuration);

builder.Services.AddApplicationServices();

var app = builder.Build();

await app.InitializeDatabase();

if (app.Environment.IsDevelopment())
    app.ConfigureSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
