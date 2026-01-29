using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderService.Domain.Entities;

namespace OrderService.Application.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        public Task<IEnumerable<OrderItem>> GetOrderItemsAsync(int orderId);
        public Task AddOrderItemAsync(int orderId, OrderItem orderItem);
        public Task DeleteOrderItemAsync(int orderItemId);
    }
}