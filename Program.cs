using DotnetApiGuideline.Sources.Application;
using DotnetApiGuideline.Sources.Infrastructure.Configurations;
using DotnetApiGuideline.Sources.Presentation.Requests;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<AppSettings>(builder.Configuration);

builder.Services.AddConfiguredControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddConfiguredSwagger();

builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderRequest>();

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
