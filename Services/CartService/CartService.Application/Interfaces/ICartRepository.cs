using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CartService.Domain.Entities;

namespace CartService.Application.Interfaces
{
    public interface ICartRepository
    {
        public Task<IEnumerable<Cart>> GetAllAsync();
        public Task<Cart?> GetAsync(int entityId);
        public Task<Cart> AddAsync(Cart entity);
        public IEnumerable<Cart> Query(Expression<Func<Cart, bool>> predicate);
        public Task DeleteAsync(int entityId);
        public Task<IEnumerable<CartItem>> GetItems(int cartId);
        public Task DeleteItem(int cartId, int itemId);
    }
}