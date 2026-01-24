using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.Entities;
using CatalogService.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Infrastructure.Repositories
{
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly CatalogDbContext _dbContext;
        public WarehouseRepository(CatalogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Warehouse entity)
        {
            await _dbContext.Warehouses.AddAsync(entity);
        }

        public Task DeleteAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Warehouse>> GetAllAsync()
        {
            var warehouses = await _dbContext.Warehouses.ToListAsync();
            return warehouses;
        }

        public async Task<Warehouse?> GetAsync(int entityId)
        {
            var warehouse = await _dbContext.Warehouses.FindAsync(entityId);
            return warehouse;
        }

        public async Task<IEnumerable<Inventory>> GetInventoryAsync(int warehouseId)
        {
            var inventory = await _dbContext.Inventory.Where(x => x.WarehouseId == warehouseId).ToListAsync();
            return inventory;
        }

        public async Task<Inventory?> GetInventoryItemAsync(int warehouseId, int inventoryId)
        {
            var inventoryItem = await _dbContext.Inventory.SingleOrDefaultAsync(x => x.WarehouseId == warehouseId && x.InventoryId == inventoryId);
            return inventoryItem;
        }

        public Task<IEnumerable<Warehouse>> QueryAsync(Expression<Func<Warehouse, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}