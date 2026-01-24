using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatalogService.Domain;

namespace CatalogService.Application.Dtos
{
    public record ProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public ProductStatus Status { get; set; }
        public bool InStock { get; set; }
    }
}