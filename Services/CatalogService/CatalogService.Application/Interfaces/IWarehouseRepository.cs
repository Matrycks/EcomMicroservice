using CatalogService.Domain.Entities;

namespace CatalogService.Application.Interfaces
{
    public interface IWarehouseRepository : IRepository<Warehouse>
    {
        public Task<IEnumerable<Inventory>> GetInventoryAsync(int warehouseId);
        public Task<Inventory?> GetInventoryItemAsync(int warehouseId, int inventoryId);
    }
}