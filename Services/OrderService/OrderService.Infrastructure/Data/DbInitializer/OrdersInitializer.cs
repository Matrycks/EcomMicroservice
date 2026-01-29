using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Data.DataContext;

namespace OrderService.Infrastructure.Data.DbInitializer
{
    public static class OrdersInitializer
    {
        public static async Task Seed(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();

            if (context.Orders.Any()) return;

            context.Orders.AddRange(
                new List<Order>
                {
                    new Order(1, new Guid(), new List<OrderItem>
                    {
                        new(1, 1),
                        new(2, 2)
                    }),
                    new Order(2, new Guid(), new List<OrderItem>
                    {
                        new(1, 2)
                    })
                }
            );

            await context.SaveChangesAsync();
        }
    }
}