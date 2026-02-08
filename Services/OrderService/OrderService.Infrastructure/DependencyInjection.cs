using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Common.Messaging.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Interfaces;
using OrderService.Infrastructure.Data;
using OrderService.Infrastructure.Data.DataContext;
using OrderService.Infrastructure.Data.Repositories;
using OrderService.Infrastructure.Messaging;
using OrderService.Infrastructure.Messaging.Interfaces;

namespace OrderService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            string connectionName,
            string? servicebusNamespace,
            bool useInMemory = true)
        {
            if (string.IsNullOrEmpty(servicebusNamespace)) throw new Exception("ServiceBus not configured");

            services.AddDbContext<OrdersDbContext>(options =>
            {
                if (useInMemory)
                    options.UseInMemoryDatabase("OrdersDb");
                else
                    options.UseSqlServer(connectionName);
            });

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddSingleton(_ => new ServiceBusClient(servicebusNamespace,
                new ManagedIdentityCredential(clientId: Environment.GetEnvironmentVariable("UAMI_CLIENT_ID"))));
            services.AddSingleton<IServiceBusPublisher, OrderMessagePublisher>();
            services.AddHostedService<CreateOrderConsumer>();
            services.AddSingleton<IMessageDispatcher, MessageDispatcher>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}