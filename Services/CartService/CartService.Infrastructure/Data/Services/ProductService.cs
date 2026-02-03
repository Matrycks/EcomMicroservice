using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CartService.Application.Interfaces;
using CartService.Domain.Dtos;

namespace CartService.Infrastructure.Data.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ProductDto> Get(int productId)
        {
            if (productId <= 0) throw new ArgumentException("ProductId must be greater than zero");

            var product = await _httpClient.GetFromJsonAsync<ProductDto>($"{productId}")
                ?? throw new NullReferenceException($"Product with Id: {productId} doesn't exist");

            return product;
        }
    }
}