
using CartService.Domain.Dtos;

namespace CartService.Domain.Entities
{
    public class CartItem
    {
        public int CartId { get; set; }
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public decimal Cost { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }

        public CartItem() { }

        public CartItem(ProductDto product, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero");

            ProductId = product.ProductId;
            Cost = product.Price;
            Quantity = quantity;

            UpdateTotal();
        }

        public void UpdateQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");

            Quantity = quantity;

            UpdateTotal();
        }

        private void UpdateTotal()
        {
            Total = Cost * Quantity;
        }
    }
}