using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CartService.Application.Interfaces;
using CartService.Domain.Entities;
using CartService.Infrastructure.Data.DataContext;
using Common.Messaging.Payloads;
using Microsoft.EntityFrameworkCore;

namespace CartService.Infrastructure.Data.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly CartDbContext _dbContext;

        public CartRepository(CartDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Cart> AddAsync(Cart entity)
        {
            await _dbContext.Carts.AddAsync(entity);
            return entity;
        }

        public async Task DeleteAsync(int entityId)
        {
            var cart = await _dbContext.Carts.FindAsync(entityId)
                ?? throw new KeyNotFoundException($"Cart with Id: {entityId} not found");

            _dbContext.Remove(cart);
        }

        public Task DeleteItem(int cartId, int itemId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Cart>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Cart?> GetAsync(int entityId)
        {
            var cart = await _dbContext.Carts.Include(x => x.Items).SingleOrDefaultAsync(x => x.CartId == entityId)
                ?? throw new KeyNotFoundException($"Cart with Id: {entityId} not found");

            return cart;
        }

        public async Task<IEnumerable<CartItem>> GetItems(int cartId)
        {
            if (cartId <= 0) throw new ArgumentException($"CartId must be greater than zero");

            var items = await _dbContext.CartItems.Where(x => x.CartId == cartId).ToListAsync();

            return items;
        }

        public IEnumerable<Cart> Query(Expression<Func<Cart, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}