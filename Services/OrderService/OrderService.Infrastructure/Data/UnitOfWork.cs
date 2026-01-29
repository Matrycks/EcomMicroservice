using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderService.Application.Interfaces;
using OrderService.Infrastructure.Data.DataContext;

namespace OrderService.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OrdersDbContext _dbContext;
        public IOrderRepository Orders { get; }

        public UnitOfWork(OrdersDbContext dbContext, IOrderRepository orders)
        {
            _dbContext = dbContext;
            Orders = orders;
        }

        public void Dispose()
        {
            this.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}