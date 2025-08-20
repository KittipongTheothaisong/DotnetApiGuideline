using System.ComponentModel.DataAnnotations.Schema;
using DotnetApiGuideline.Sources.Domain.Enums;
using DotnetApiGuideline.Sources.Domain.ValueObjects;

namespace DotnetApiGuideline.Sources.Domain.Entities;

[Table("orders")]
public class OrderEntity : BaseEntity
{
    public string OrderNumber { get; set; } = string.Empty;
    public CustomerEntity Customer { get; set; } = new CustomerEntity();
    public IEnumerable<OrderItemEntity> Items { get; set; } = [];
    public OrderStatus Status { get; set; } = OrderStatus.Unknown;
    public Address ShippingAddress { get; set; } = Address.Empty();
    public string? Notes { get; set; }

    public Money TotalAmount =>
        Items.Aggregate(Money.Zero(), (total, item) => total + item.TotalPrice);
}
