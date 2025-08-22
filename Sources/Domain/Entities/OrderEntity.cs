using System.ComponentModel.DataAnnotations.Schema;
using DotnetApiGuideline.Sources.Domain.Enums;
using DotnetApiGuideline.Sources.Domain.ValueObjects;

namespace DotnetApiGuideline.Sources.Domain.Entities;

[Table("orders")]
public class OrderEntity : BaseEntity
{
    public string OrderNumber { get; set; } = string.Empty;
    public CustomerEntity Customer { get; set; } = new CustomerEntity();
    public ICollection<OrderItemEntity> Items { get; set; } = [];
    public OrderStatus Status { get; private set; } = OrderStatus.Unknown;
    public Address ShippingAddress { get; set; } = Address.Empty();
    public string? Notes { get; set; }

    public OrderEntity() { }

    public OrderEntity(
        string orderNumber,
        CustomerEntity customer,
        Address shippingAddress,
        ICollection<OrderItemEntity> items,
        OrderStatus status,
        string? notes
    )
    {
        OrderNumber = orderNumber;
        Customer = customer;
        ShippingAddress = shippingAddress;
        Items = items;
        Status = status;
        Notes = notes;
    }

    public Money TotalAmount =>
        Items.Aggregate(Money.Zero(), (total, item) => total + item.TotalPrice);

    public void ChangeStatus(OrderStatus newStatus)
    {
        if (!IsValidStatusTransition(Status, newStatus))
            throw new Exception($"Cannot transition from {Status} to {newStatus}");

        Status = newStatus;
    }

    private static bool IsValidStatusTransition(OrderStatus current, OrderStatus next)
    {
        return (current, next) switch
        {
            (OrderStatus.Pending, OrderStatus.Confirmed) => true,
            (OrderStatus.Confirmed, OrderStatus.Shipped) => true,
            (OrderStatus.Shipped, OrderStatus.Delivered) => true,
            (_, OrderStatus.Cancelled) => current != OrderStatus.Delivered,
            _ => false,
        };
    }
}
