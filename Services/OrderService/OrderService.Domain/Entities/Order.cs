
namespace OrderService.Domain.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int? CartId { get; set; }
        public Guid OrderNumber { get; set; }
        public ICollection<OrderItem> Items { get; set; } = null!;
        public bool IsPaid { get; set; }
        public decimal Total { get; private set; }
        public OrderStatus Status { get; private set; }
        public DateTime CreatedDate { get; set; }

        public Order() { }
        public Order(int customerId, Guid orderNumber, IEnumerable<OrderItem> orderItems)
        {
            if (customerId <= 0) throw new ArgumentException("Invalid customerId");
            if (!orderItems.Any()) throw new Exception("Orders cannot be created without items");

            //CartId = cartId;
            CustomerId = customerId;
            OrderNumber = orderNumber;
            CreatedDate = DateTime.Now.ToUniversalTime();
            Items = [.. orderItems];

            SetStatus(OrderStatus.Pending);

            SetTotal();
        }

        public void SetStatus(OrderStatus newStatus)
        {
            if (IsPaid) throw new Exception("Order is paid, status cannot be changed");

            if (newStatus == OrderStatus.Paid) IsPaid = true;

            Status = newStatus;
        }

        public void Cancel()
        {
            if (Status == OrderStatus.Shipped || Status == OrderStatus.Delivered)
                throw new Exception("Order delivered and cannot be cancelled");

            Status = OrderStatus.Cancelled;
        }

        private void SetTotal()
        {
            decimal nTotal = Items.Sum(x => x.Total);
            Total = nTotal;
        }
    }
}