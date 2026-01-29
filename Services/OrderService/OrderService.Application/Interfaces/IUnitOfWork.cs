using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IOrderRepository Orders { get; }
        public Task SaveChangesAsync();
    }
}