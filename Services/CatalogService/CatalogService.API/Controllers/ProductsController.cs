using CatalogService.Application.Dtos;
using CatalogService.Application.Products;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.API.Controllers;

/// <summary>
/// Manages product catalog.
/// </summary>
[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ILogger<ProductsController> _logger;
    private readonly IMediator _mediator;

    /// <summary>
    /// Manages product catalog.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="mediator"></param>
    public ProductsController(ILogger<ProductsController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    /// <summary>
    /// Retreives a list of products.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _mediator.Send(new GetProductsRequest());
        return Ok(products.Adapt<IEnumerable<ProductDto>>());
    }

    /// <summary>
    /// Retreives a product by ProductId.
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    [HttpGet("{productId}")]
    public async Task<IActionResult> GetProduct(int productId)
    {
        var product = await _mediator.Send(new GetProductRequest(productId));
        return Ok(product.Adapt<ProductDto>());
    }
}
