using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatalogService.Application.Interfaces;
using CatalogService.Infrastructure.DataContext;
using CatalogService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            string connectionName,
            bool useInMemory
        )
        {
            if (useInMemory)
                services.AddDbContext<CatalogDbContext>(options => options.UseInMemoryDatabase("CatalogDb"));
            else
                services.AddDbContext<CatalogDbContext>(options => options.UseSqlServer(connectionName));

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IWarehouseRepository, WarehouseRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}