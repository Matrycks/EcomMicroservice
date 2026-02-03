
namespace CartService.Domain.Entities
{
    public class Cart
    {
        public int CartId { get; set; }
        public int CustomerId { get; set; }
        public decimal Total { get; private set; }
        public bool IsPaid { get; private set; } = false;
        public DateTime CreatedDate { get; set; }
        public ICollection<CartItem> Items { get; set; } = [];

        public Cart() { }

        public Cart(int customerId, ICollection<CartItem> items)
        {
            if (customerId <= 0) throw new ArgumentException("CustomerId must be greater than 0");

            CustomerId = customerId;
            Items = items;
            CreatedDate = DateTime.UtcNow;

            SetTotal();
        }

        public void AddItem(CartItem item)
        {
            if (IsPaid) throw new Exception("Order is paid and cannot be changed");
            if (Items.Any(x => x.ProductId == item.ProductId))
            {
                var existingItem = Items.Single(x => x.ProductId == item.ProductId);
                existingItem.UpdateQuantity(existingItem.Quantity + item.Quantity);
            }
            else
                Items.Add(item);

            SetTotal();
        }

        public void RemoveItem(CartItem item)
        {
            if (IsPaid) throw new Exception("Order is paid and cannot be changed");

            Items.Remove(item);

            SetTotal();
        }

        private void SetTotal()
        {
            decimal nTotal = Items.Sum(x => x.Total);
            Total = nTotal;
        }

        public void UpdateOrderItemQuantity(int productId, int quantity)
        {
            CartItem cartItem = (CartItem?)Items.SingleOrDefault(x => x.ProductId == productId)
                ?? throw new KeyNotFoundException("No item found with productId");
            cartItem.UpdateQuantity(quantity);

            SetTotal();
        }

        public void Clear()
        {
            Items.Clear();

            SetTotal();
        }
    }
}