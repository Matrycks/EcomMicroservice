using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.Entities;
using MediatR;

namespace CatalogService.Application.Products
{
    public record GetProductRequest(int ProductId) : IRequest<Product>;

    public class GetProduct : IRequestHandler<GetProductRequest, Product>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetProduct(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Product> Handle(GetProductRequest request, CancellationToken cancellationToken)
        {
            if (request.ProductId <= 0) throw new ArgumentException("Invalid ProductId passed");

            var product = await _unitOfWork.Products.GetAsync(request.ProductId)
                ?? throw new NullReferenceException($"Product: {request.ProductId} not found");
            return product;
        }
    }
}