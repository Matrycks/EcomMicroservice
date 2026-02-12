using CartService.Application.Carts;
using CartService.Application.Dtos;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CartService.API.Controllers;

/// <summary>
/// Manages carts within an ecommerce system.
/// </summary>
[ApiController]
[Route("[controller]")]
public class CartsController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Manages carts within an ecommerce system.
    /// </summary>
    /// <param name="mediator"></param>
    public CartsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Creates a new shopping cart.
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateCart(CreateCartCommand command)
    {
        var nCart = await _mediator.Send(command);

        return Ok(nCart.Adapt<CartDto>());
    }

    /// <summary>
    /// Handles checkout process for a cart.
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout(CartCheckoutCommand command)
    {
        await _mediator.Send(command);

        return NoContent();
    }
}
