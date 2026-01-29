using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Data.DataContext;

namespace OrderService.Infrastructure.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrdersDbContext _dbContext;

        public OrderRepository(OrdersDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Order> AddAsync(Order entity)
        {
            await _dbContext.Orders.AddAsync(entity);
            return entity;
        }

        public async Task AddOrderItemAsync(int orderId, OrderItem orderItem)
        {
            var order = await _dbContext.Orders.FindAsync(orderId)
                ?? throw new KeyNotFoundException($"Order: {orderId} not found");

            order.Items.Add(orderItem);
        }

        public async Task DeleteAsync(int entityId)
        {
            var order = await _dbContext.Orders.FindAsync(entityId)
                ?? throw new KeyNotFoundException($"Order: {entityId} not found");

            _dbContext.Orders.Remove(order);
        }

        public async Task DeleteOrderItemAsync(int orderItemId)
        {
            var orderItem = await _dbContext.OrderItems.FindAsync(orderItemId)
                ?? throw new KeyNotFoundException($"OrderItem: {orderItemId} not found");

            _dbContext.OrderItems.Remove(orderItem);
        }

        public Task<IEnumerable<Order>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Order?> GetAsync(int entityId)
        {
            var order = await _dbContext.Orders.FindAsync(entityId)
                ?? throw new KeyNotFoundException($"Order: {entityId} not found");

            return order;
        }

        public async Task<IEnumerable<OrderItem>> GetOrderItemsAsync(int orderId)
        {
            var order = await _dbContext.Orders.FindAsync(orderId)
                ?? throw new KeyNotFoundException($"Order: {orderId} not found");

            var orderItems = await _dbContext.OrderItems.Where(x => x.OrderId == orderId).ToListAsync();
            return orderItems;
        }

        public IEnumerable<Order> Query(Expression<Func<Order, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}