using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogService.Domain.Entities
{
    public class Warehouse
    {
        public int WarehouseId { get; set; }
        public string Location { get; set; } = null!;
        public string Name { get; set; } = null!;
        public ICollection<Inventory> Inventory { get; set; } = [];

        public Warehouse() { }
        public Warehouse(string location, string name)
        {
            if (string.IsNullOrEmpty(location) || string.IsNullOrEmpty(name))
                throw new ArgumentException("Cannot create Warehouse without required params");

            Location = location;
            Name = name;
        }

        public void AddInventory(Inventory inventory)
        {
            if (inventory.WarehouseId != WarehouseId)
                throw new ArgumentException("Invalid Warehouse ID, when adding inventory");

            if (Inventory.Any(x => x.ProductId == inventory.ProductId))
            {
                var existingInv = Inventory.FirstOrDefault(x => x.ProductId == inventory.ProductId)!;
                existingInv.IncreaseQuantity(inventory.Quantity);
            }
            else
            {
                Inventory.Add(inventory);
            }
        }

        public void RemoveInventory(int inventoryId)
        {
            if (inventoryId <= 0)
                throw new ArgumentException("Invalid Inventory ID when removing warehouse inventory");

            if (!Inventory.Any(x => x.InventoryId == inventoryId))
                throw new NullReferenceException("Inventory does not exist within warehouse");

            Inventory.Remove(Inventory.Single(x => x.InventoryId == inventoryId));
        }
    }
}