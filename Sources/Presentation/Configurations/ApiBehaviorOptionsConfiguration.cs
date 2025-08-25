using DotnetApiGuideline.Sources.Presentation.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DotnetApiGuideline.Sources.Presentation.Configurations;

public class ApiBehaviorOptionsConfiguration : IConfigureOptions<ApiBehaviorOptions>
{
    public void Configure(ApiBehaviorOptions options)
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context
                .ModelState.Where(x => x.Value?.Errors.Count > 0)
                .SelectMany(x => x.Value?.Errors ?? [])
                .Select(x => x.ErrorMessage);

            var errorResponse = new ErrorResponse(
                Type: "ValidationError",
                Title: "One or more validation errors occurred",
                Message: string.Join(Environment.NewLine, errors)
            );

            var result = new BadRequestObjectResult(errorResponse);
            result.ContentTypes.Add("application/json");

            return result;
        };
    }
}
