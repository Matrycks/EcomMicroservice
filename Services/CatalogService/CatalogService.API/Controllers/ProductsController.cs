using CatalogService.Application.Dtos;
using CatalogService.Application.Products;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ILogger<ProductsController> _logger;
    private readonly IMediator _mediator;

    public ProductsController(ILogger<ProductsController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _mediator.Send(new GetProductsRequest());
        return Ok(products.Adapt<IEnumerable<ProductDto>>());
    }

    [HttpGet("{productId}")]
    public async Task<IActionResult> GetProduct(int productId)
    {
        var product = await _mediator.Send(new GetProductRequest(productId));
        return Ok(product.Adapt<ProductDto>());
    }
}
