using CartService.Application.Interfaces;
using CartService.Infrastructure.Data;
using CartService.Infrastructure.Data.DataContext;
using CartService.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CartService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        string dbConnectionString,
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

        return services;
    }
}
