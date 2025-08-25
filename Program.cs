using DotnetApiGuideline.Sources.Application;
using DotnetApiGuideline.Sources.Infrastructure.Configurations;
using DotnetApiGuideline.Sources.Presentation.Configurations;
using DotnetApiGuideline.Sources.Presentation.Middlewares;
using DotnetApiGuideline.Sources.Presentation.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<AppSettings>(builder.Configuration);

builder.Services.AddConfiguredControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddConfiguredSwagger();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderRequestValidator>();
builder.Services.ConfigureOptions<ApiBehaviorOptionsConfiguration>();

builder.Services.AddConfiguredAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services.AddConfiguredDatabase(builder.Configuration);
builder.Services.AddConfiguredRepositories(builder.Configuration);

builder.Services.AddConfiguredServices();

var app = builder.Build();

await app.ConfigureSeedDataAsync();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
    app.ConfigureSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
