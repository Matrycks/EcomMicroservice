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
    public class ProductRepository : IProductRepository
    {
        private readonly CatalogDbContext _dbContext;
        public ProductRepository(CatalogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Product entity)
        {
            await _dbContext.Products.AddAsync(entity);
        }

        public Task DeleteAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var products = await _dbContext.Products.ToListAsync();
            return products;
        }

        public async Task<Product?> GetAsync(int entityId)
        {
            var product = await _dbContext.Products.FindAsync(entityId);
            return product;
        }

        public Task<IEnumerable<Product>> QueryAsync(Expression<Func<Product, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}