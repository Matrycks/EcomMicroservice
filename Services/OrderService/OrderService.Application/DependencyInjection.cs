using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Common.Messaging.Orders;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Messaging.Interfaces;
using OrderService.Application.Orders;

namespace OrderService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddScoped<GetOrder>();
            services.AddScoped<GetOrders>();
            services.AddScoped<IMessageHandler<CreateOrderMessage>, CreateOrderHandler>();

            return services;
        }
    }
}