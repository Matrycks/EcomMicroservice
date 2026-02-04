using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Messaging.Payloads;

namespace Common.Messaging.Orders
{
    public record CreateOrderMessage
    {
        public int CustomerId { get; set; }
        public ICollection<OrderItem> Items { get; set; } = null!;
        public decimal Total { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}