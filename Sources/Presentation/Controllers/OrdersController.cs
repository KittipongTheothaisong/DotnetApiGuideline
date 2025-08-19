using DotnetApiGuideline.Sources.Application.Interfaces;
using DotnetApiGuideline.Sources.Domain.Enums;
using DotnetApiGuideline.Sources.Presentation.Requests;
using DotnetApiGuideline.Sources.Presentation.Responses;
using Microsoft.AspNetCore.Mvc;

namespace DotnetApiGuideline.Sources.Presentation.Controllers;

[ApiController]
[Route("api/orders")]
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
}
