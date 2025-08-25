using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using DotnetApiGuideline.Sources.Domain.Exceptions;
using DotnetApiGuideline.Sources.Presentation.Responses;

namespace DotnetApiGuideline.Sources.Presentation.Middlewares;

public class GlobalExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<GlobalExceptionHandlingMiddleware> logger
)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger = logger;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var errorResponse = MapExceptionToErrorResponse(exception);
        response.StatusCode = MapExceptionToStatusCode(exception);

        var jsonResponse = JsonSerializer.Serialize(errorResponse, JsonOptions);
        await response.WriteAsync(jsonResponse);
    }

    private static ErrorResponse MapExceptionToErrorResponse(Exception exception)
    {
        return exception switch
        {
            ValidationException validationEx => new ErrorResponse(
                Type: "ValidationError",
                Title: "Validation failed",
                Message: validationEx.Message
            ),
            DomainException domainEx => new ErrorResponse(
                Type: "DomainError",
                Title: "Domain rule violation",
                Message: domainEx.Message
            ),
            KeyNotFoundException => new ErrorResponse(
                Type: "NotFound",
                Title: "Resource not found",
                Message: exception.Message
            ),
            UnauthorizedAccessException => new ErrorResponse(
                Type: "Unauthorized",
                Title: "Unauthorized access",
                Message: "You are not authorized to perform this action"
            ),
            _ => new ErrorResponse(
                Type: "InternalServerError",
                Title: "An error occurred while processing your request",
                Message: "Something went wrong. Please try again later."
            ),
        };
    }

    private static int MapExceptionToStatusCode(Exception exception) =>
        exception switch
        {
            ValidationException => (int)HttpStatusCode.BadRequest,
            DomainException => (int)HttpStatusCode.BadRequest,
            KeyNotFoundException => (int)HttpStatusCode.NotFound,
            UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
            _ => (int)HttpStatusCode.InternalServerError,
        };
}
