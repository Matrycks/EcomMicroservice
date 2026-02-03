using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartService.Application.Dtos
{
    public record CartItemDto
    {
        public int CartId { get; set; }
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public decimal Cost { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
    }
}