using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using MyWebApp.Models;

namespace MyWebApp.Data
{
  public class StoreContext : DbContext
  {
    public StoreContext(DbContextOptions<StoreContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Product { get; set; } = default!;
    public DbSet<Category> Category { get; set; } = default!;
    public DbSet<ProductCategory> ProductCategory => Set<ProductCategory>("ProductCategory");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      // Seed Data
      modelBuilder.Entity<Product>().HasData(
        new Product
        {
          ProductId = 1,
          ProductName = "Product 1",
          Manufacture = "Manufacture 1",
        },
        new Product
        {
          ProductId = 2,
          ProductName = "Product 2",
          Manufacture = "Manufacture 2"
        },
        new Product
        {
          ProductId = 3,
          ProductName = "Product 3",
          Manufacture = "Manufacture 3"
        },
        new Product
        {
          ProductId = 4,
          ProductName = "Product 4",
          Manufacture = "Manufacture 4"
        },
        new Product
        {
          ProductId = 5,
          ProductName = "Product 5",
          Manufacture = "Manufacture 5"
        }
      );
      modelBuilder.Entity<Category>().HasData(
        new Category
        {
          CategoryId = 1,
          CategoryName = "Category 1",
        },
        new Category
        {
          CategoryId = 2,
          CategoryName = "Category 2"
        },
        new Category
        {
          CategoryId = 3,
          CategoryName = "Category 3"
        },
        new Category
        {
          CategoryId = 4,
          CategoryName = "Category 4"
        },
        new Category
        {
          CategoryId = 5,
          CategoryName = "Category 5"
        }
      );
      modelBuilder.Entity<Product>().HasMany(p => p.Categories).WithMany(c => c.Products).UsingEntity<ProductCategory>("ProductCategory",
        p => p.HasOne<Category>().WithMany().HasForeignKey("CategoryId"),
        c => c.HasOne<Product>().WithMany().HasForeignKey("ProductId"),
        j =>
        {
          j.HasKey("ProductId", "CategoryId");
          j.HasData(
            new { ProductId = 1, CategoryId = 1 },
            new { ProductId = 1, CategoryId = 2 },
            new { ProductId = 1, CategoryId = 3 },
            new { ProductId = 2, CategoryId = 2 },
            new { ProductId = 2, CategoryId = 3 },
            new { ProductId = 2, CategoryId = 4 },
            new { ProductId = 3, CategoryId = 3 },
            new { ProductId = 3, CategoryId = 4 },
            new { ProductId = 3, CategoryId = 5 },
            new { ProductId = 4, CategoryId = 4 },
            new { ProductId = 4, CategoryId = 5 },
            new { ProductId = 4, CategoryId = 1 },
            new { ProductId = 5, CategoryId = 5 },
            new { ProductId = 5, CategoryId = 1 },
            new { ProductId = 5, CategoryId = 2 }
          );
        }
      );
    }
  }
}
