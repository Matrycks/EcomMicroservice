using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Dtos;
using OrderService.Application.Orders;

namespace OrderService.API.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        var orders = await _mediator.Send(new GetOrdersRequest());
        return Ok(orders.Adapt<IEnumerable<OrderDto>>());
    }

    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetOrder(int orderId)
    {
        var order = await _mediator.Send(new GetOrderRequest(orderId));
        return Ok(order.Adapt<OrderDto>());
    }
}
