using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartService.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public ICartRepository Carts { get; }
        public Task SaveChangesAsync();
    }
}