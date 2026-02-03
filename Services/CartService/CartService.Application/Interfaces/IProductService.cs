using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CartService.Domain.Dtos;

namespace CartService.Application.Interfaces
{
    public interface IProductService
    {
        public Task<ProductDto> Get(int productId);
    }
}