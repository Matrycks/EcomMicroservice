using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartService.Application.Dtos
{
    public record CartDto
    {
        public int CartId { get; set; }
        public int CustomerId { get; set; }
        public decimal Total { get; private set; }
        public bool IsPaid { get; private set; } = false;
        public DateTime CreatedDate { get; set; }
        public ICollection<CartItemDto> Items { get; set; } = [];
    }
}