namespace DotnetApiGuideline.Sources.Presentation.Requests;

public record OrderItemRequest(Guid ProductId, int Quantity);
