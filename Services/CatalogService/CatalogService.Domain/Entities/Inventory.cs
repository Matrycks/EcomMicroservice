using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogService.Domain.Entities
{
    public class Inventory
    {
        public int InventoryId { get; set; }
        public int ProductId { get; set; }
        public int WarehouseId { get; set; }
        public int Quantity { get; set; }

        public Inventory() { }

        public Inventory(int productId, int warehouseId, int quantity)
        {
            if (productId <= 0 || warehouseId <= 0 || quantity <= 0)
                throw new ArgumentException("Inventory has invalid params");

            ProductId = productId;
            WarehouseId = warehouseId;
            Quantity = quantity;
        }

        public void IncreaseQuantity(int amount)
        {
            if (amount <= 0) throw new ArgumentException("Invalid param when increasing Inventory quantity");

            Quantity += amount;
        }

        public void DecreaseQuantity(int amount)
        {
            if (amount <= 0) throw new ArgumentException("Invalid param when decreasing Inventory quantity");

            Quantity -= amount;
        }
    }
}