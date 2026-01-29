using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Application.Dtos
{
    public record OrderItemDto
    {
        public int OrderItemId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public int OrderId { get; set; }
        public decimal Cost { get; set; }
    }
}