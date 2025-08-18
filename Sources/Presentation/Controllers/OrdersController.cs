using DotnetApiGuideline.Sources.Application.Interfaces;
using DotnetApiGuideline.Sources.Application.Models;
using DotnetApiGuideline.Sources.Domain.Enums;
using DotnetApiGuideline.Sources.Presentation.Requests;
using DotnetApiGuideline.Sources.Presentation.Responses;
using Microsoft.AspNetCore.Mvc;

namespace DotnetApiGuideline.Sources.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController(IOrderService orderService) : ControllerBase
{
    private readonly IOrderService _orderService = orderService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrders()
    {
        var orders = await _orderService.GetOrdersAsync();
        return Ok(orders.Select(OrderResponse.FromOrder));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderResponse>> GetOrderById(Guid id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        return Ok(OrderResponse.FromOrder(order));
    }

    [HttpGet("by_order_number/{orderNumber}")]
    public async Task<ActionResult<OrderResponse>> GetOrderByOrderNumber(string orderNumber)
    {
        var order = await _orderService.GetOrderByOrderNumberAsync(orderNumber);
        return Ok(OrderResponse.FromOrder(order));
    }

    [HttpGet("by_customer/{customerEmail}")]
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrdersByCustomerEmail(
        string customerEmail
    )
    {
        var orders = await _orderService.GetOrdersByCustomerEmailAsync(customerEmail);
        return Ok(orders.Select(OrderResponse.FromOrder));
    }

    [HttpGet("by_status/{status}")]
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrdersByStatus(
        OrderStatus status
    )
    {
        var orders = await _orderService.GetOrdersByStatusAsync(status);
        return Ok(orders.Select(OrderResponse.FromOrder));
    }

    [HttpPost]
    public async Task<ActionResult<OrderResponse>> CreateOrder(CreateOrderRequest createRequest)
    {
        var order = await _orderService.CreateOrderAsync(createRequest);

        return Ok(OrderResponse.FromOrder(order));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<OrderResponse>> UpdateOrder(
        Guid id,
        UpdateOrderRequest updateRequest
    )
    {
        var order = await _orderService.UpdateOrderAsync(id, updateRequest);
        return Ok(OrderResponse.FromOrder(order));
    }

    [HttpPatch("{id}/status")]
    public async Task<ActionResult<OrderResponse>> UpdateOrderStatus(
        Guid id,
        [FromBody] OrderStatus status
    )
    {
        var order = await _orderService.UpdateOrderStatusAsync(id, status);
        return Ok(OrderResponse.FromOrder(order));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(Guid id)
    {
        await _orderService.DeleteOrderAsync(id);
        return NoContent();
    }

    [HttpGet("order_number_exists/{orderNumber}")]
    public async Task<ActionResult<bool>> OrderNumberExists(string orderNumber)
    {
        var exists = await _orderService.OrderNumberExistsAsync(orderNumber);
        return Ok(exists);
    }
}
