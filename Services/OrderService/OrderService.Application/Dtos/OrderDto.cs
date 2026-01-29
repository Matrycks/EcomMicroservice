using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderService.Domain;

namespace OrderService.Application.Dtos
{
    public record OrderDto
    {
        public int OrderId { get; set; }
        public Guid OrderNumber { get; set; }
        public ICollection<OrderItemDto> Items { get; set; } = null!;
        public int CustomerId { get; set; }
        public decimal Total { get; set; }
        public bool IsPaid { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}