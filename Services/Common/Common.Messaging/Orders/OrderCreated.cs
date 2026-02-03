using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Messaging.Orders
{
    public record OrderCreatedMessage
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}