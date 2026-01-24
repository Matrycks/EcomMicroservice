using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatalogService.Application.Interfaces;
using CatalogService.Infrastructure.DataContext;

namespace CatalogService.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CatalogDbContext _dbContext;
        public IProductRepository Products { get; }
        public readonly IWarehouseRepository Warehouses;

        public UnitOfWork(CatalogDbContext dbContext, IProductRepository productRepository, IWarehouseRepository warehouses)
        {
            _dbContext = dbContext;
            Products = productRepository;
            Warehouses = warehouses;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}