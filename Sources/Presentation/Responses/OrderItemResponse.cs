using DotnetApiGuideline.Sources.Application.Models;

namespace DotnetApiGuideline.Sources.Presentation.Responses;

public record OrderItemResponse(
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice
)
{
    public static OrderItemResponse FromOrderItem(OrderItem orderItem)
    {
        return new OrderItemResponse(
            ProductName: orderItem.Product.Name,
            Quantity: orderItem.Quantity,
            UnitPrice: orderItem.UnitPrice.Amount,
            TotalPrice: orderItem.TotalPrice.Amount
        );
    }
};
