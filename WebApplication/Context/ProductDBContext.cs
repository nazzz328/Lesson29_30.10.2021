using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Context
{
    public class ProductDBContext : DbContext
    {
        public ProductDBContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductCategory>().HasData
                (
                new ProductCategory { Id = 1, Name = "Овощи" },
                new ProductCategory { Id = 2, Name = "Фрукты" },
                new ProductCategory { Id = 3, Name = "Мясо" },
                new ProductCategory { Id = 4, Name = "Крупы" },
                new ProductCategory { Id = 5, Name = "Напитки" },
                new ProductCategory { Id = 6, Name = "Прочее" }
                );
        }

        public DbSet<Product> Products { get; set; } 
        public DbSet<ProductCategory> ProductCategories { get; set; }
    }
}
