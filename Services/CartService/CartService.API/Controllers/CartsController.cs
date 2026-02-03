using CartService.Application.Carts;
using CartService.Application.Dtos;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CartService.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CartsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CartsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCart(CreateCartCommand command)
    {
        var nCart = await _mediator.Send(command);

        return Ok(nCart.Adapt<CartDto>());
    }
}
