using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatalogService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Infrastructure.DataContext
{
    public class CatalogDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Inventory> Inventory { get; set; }

        public CatalogDbContext(DbContextOptions<CatalogDbContext> options)
            : base(options)
        {

        }
    }
}