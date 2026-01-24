using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.Entities;
using MediatR;

namespace CatalogService.Application.Products
{
    public record GetProductsRequest() : IRequest<IEnumerable<Product>>;

    public class GetProducts : IRequestHandler<GetProductsRequest, IEnumerable<Product>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetProducts(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Product>> Handle(GetProductsRequest request, CancellationToken cancellationToken)
        {
            var products = await _unitOfWork.Products.GetAllAsync();
            return products;
        }
    }
}