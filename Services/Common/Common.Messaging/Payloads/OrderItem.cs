using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Messaging.Payloads
{
    public class OrderItem
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public decimal Cost { get; set; }
    }
}