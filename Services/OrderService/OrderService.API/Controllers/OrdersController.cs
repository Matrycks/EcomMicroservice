using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Dtos;
using OrderService.Application.Orders;

namespace OrderService.API.Controllers;

/// <summary>
/// Manages orders within an ecommerce system.
/// </summary>
[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Manages orders within an ecommerce system.
    /// </summary>
    /// <param name="mediator"></param>
    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retreives a list of orders.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        var orders = await _mediator.Send(new GetOrdersRequest());
        return Ok(orders.Adapt<IEnumerable<OrderDto>>());
    }

    /// <summary>
    /// Retreives an order by OrderId.
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetOrder(int orderId)
    {
        var order = await _mediator.Send(new GetOrderRequest(orderId));
        return Ok(order.Adapt<OrderDto>());
    }
}
