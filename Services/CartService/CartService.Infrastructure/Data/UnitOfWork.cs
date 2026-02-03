using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CartService.Application.Interfaces;
using CartService.Infrastructure.Data.DataContext;

namespace CartService.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CartDbContext _dbContext;
        public ICartRepository Carts { get; }

        public UnitOfWork(CartDbContext dbContext, ICartRepository cartRepository)
        {
            _dbContext = dbContext;
            Carts = cartRepository;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}