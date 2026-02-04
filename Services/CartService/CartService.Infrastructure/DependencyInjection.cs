using Azure.Identity;
using Azure.Messaging.ServiceBus;
using CartService.Application.Interfaces;
using CartService.Application.Messaging;
using CartService.Infrastructure.Data;
using CartService.Infrastructure.Data.DataContext;
using CartService.Infrastructure.Data.Repositories;
using CartService.Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CartService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        string dbConnectionString,
        string? servicebusNamespace,
        bool useInMemory = true)
    {
        services.AddDbContext<CartDbContext>(cfg =>
        {
            if (useInMemory)
                cfg.UseInMemoryDatabase("CartsDb");
            else
                cfg.UseSqlServer(dbConnectionString);
        });

        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddSingleton(_ => new ServiceBusClient(servicebusNamespace, new DefaultAzureCredential()));
        services.AddSingleton<IServiceBusPublisher, OrderMessagePublisher>();

        return services;
    }
}
