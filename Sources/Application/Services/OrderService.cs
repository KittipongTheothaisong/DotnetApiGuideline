using System.ComponentModel.DataAnnotations;
using DotnetApiGuideline.Sources.Application.Interfaces;
using DotnetApiGuideline.Sources.Application.Models;
using DotnetApiGuideline.Sources.Domain.Entities;
using DotnetApiGuideline.Sources.Domain.Enums;
using DotnetApiGuideline.Sources.Domain.Interfaces;
using DotnetApiGuideline.Sources.Domain.ValueObjects;
using DotnetApiGuideline.Sources.Presentation.Requests;

namespace DotnetApiGuideline.Sources.Application.Services;

public class OrderService(
    IOrderRepository orderRepository,
    ICustomerRepository customerRepository,
    IProductRepository productRepository
) : IOrderService
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly ICustomerRepository _customerRepository = customerRepository;
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<Order> CreateOrderAsync(CreateOrderRequest request)
    {
        var orderNumber = await GenerateOrderNumberAsync();
        var customer = await _customerRepository.GetCustomerByIdAsync(request.CustomerId);
        var products = await _productRepository.GetProductsByIdsAsync(
            request.Items.Select(item => item.ProductId)
        );

        await EnsureCanCreateOrderAsync(request, products);

        var order = new OrderEntity(
            orderNumber: orderNumber,
            customer: customer,
            items:
            [
                .. request.Items.Select(item =>
                {
                    var product =
                        products.FirstOrDefault(p => p.Id == item.ProductId)
                        ?? throw new KeyNotFoundException(
                            $"Product with ID {item.ProductId} not found."
                        );

                    return OrderItemEntity.FromProduct(product, item.Quantity);
                }),
            ],
            status: OrderStatus.Pending,
            shippingAddress: Address.FromRequest(request.ShippingAddress),
            notes: request.Notes
        );

        var createdOrder = await _orderRepository.CreateOrderAsync(order);

        return Order.FromEntity(createdOrder);
    }

    public async Task DeleteOrderAsync(Guid id)
    {
        await _orderRepository.DeleteOrderAsync(id);
    }

    public async Task<Order> GetOrderByIdAsync(Guid id)
    {
        var order = await _orderRepository.GetOrderByIdAsync(id);
        return Order.FromEntity(order);
    }

    public async Task<Order> GetOrderByOrderNumberAsync(string orderNumber)
    {
        var order = await _orderRepository.GetOrderByOrderNumberAsync(orderNumber);
        return Order.FromEntity(order);
    }

    public async Task<IEnumerable<Order>> GetOrdersAsync()
    {
        var orders = await _orderRepository.GetOrdersAsync();
        return orders.Select(Order.FromEntity);
    }

    public async Task<IEnumerable<Order>> GetOrdersByCustomerEmailAsync(string customerEmail)
    {
        var orders = await _orderRepository.GetOrdersByCustomerEmailAsync(customerEmail);
        return orders.Select(Order.FromEntity);
    }

    public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status)
    {
        var orders = await _orderRepository.GetOrdersByStatusAsync(status);
        return orders.Select(Order.FromEntity);
    }

    public async Task<bool> OrderNumberExistsAsync(string orderNumber)
    {
        return await _orderRepository.OrderNumberExistsAsync(orderNumber);
    }

    public async Task<Order> UpdateOrderAsync(Guid id, UpdateOrderRequest request)
    {
        var order = await _orderRepository.GetOrderByIdAsync(id);
        var address =
            request.ShippingAddress != null ? Address.FromRequest(request.ShippingAddress) : null;

        order.ShippingAddress = address ?? order.ShippingAddress;
        order.Notes = request.Notes ?? order.Notes;

        if (request.Items != null && request.Items.Count != 0)
        {
            var products = await _productRepository.GetProductsByIdsAsync(
                request.Items.Select(item => item.ProductId)
            );

            order.Items =
            [
                .. request.Items.Select(item =>
                {
                    var product =
                        products.FirstOrDefault(p => p.Id == item.ProductId)
                        ?? throw new KeyNotFoundException(
                            $"Product with ID {item.ProductId} not found."
                        );

                    return OrderItemEntity.FromProduct(product, item.Quantity);
                }),
            ];
        }

        await _orderRepository.UpdateOrderAsync(order);

        return Order.FromEntity(order);
    }

    public async Task<Order> UpdateOrderStatusAsync(Guid id, OrderStatus status)
    {
        var order = await _orderRepository.GetOrderByIdAsync(id);
        order.ChangeStatus(status);

        await _orderRepository.UpdateOrderAsync(order);

        return Order.FromEntity(order);
    }

    private static async Task<string> GenerateOrderNumberAsync()
    {
        return await Task.FromResult(Guid.NewGuid().ToString());
    }

    private async Task EnsureCanCreateOrderAsync(
        CreateOrderRequest request,
        IEnumerable<ProductEntity> products
    )
    {
        var customer =
            await _customerRepository.GetCustomerByIdAsync(request.CustomerId)
            ?? throw new ValidationException("Customer not found");

        if (!customer.IsActive)
            throw new ValidationException("Customer account is not active");

        foreach (var item in request.Items)
        {
            var product =
                products.FirstOrDefault(p => p.Id == item.ProductId)
                ?? throw new ValidationException($"Product {item.ProductId} not found");

            if (product.StockQuantity < item.Quantity)
                throw new ValidationException($"Insufficient stock for product {product.Name}");
        }
    }
}
