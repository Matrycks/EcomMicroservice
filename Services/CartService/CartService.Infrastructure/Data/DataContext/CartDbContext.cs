using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CartService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CartService.Infrastructure.Data.DataContext
{
    public class CartDbContext : DbContext
    {
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        public CartDbContext(DbContextOptions<CartDbContext> options)
            : base(options) { }
    }
}