using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Chim_En_DOTNET.Models;

namespace Chim_En_DOTNET.Data
{
  public class ApplicationDbContext : IdentityDbContext
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Product> Products { get; set; }
    public DbSet<Cart> Carts { get; set; }

    public DbSet<CartItem> CartItems { get; set; }

    public DbSet<ProductImage> ProductImages { get; set; }

    public DbSet<Category> Categories { get; set; }



    public DbSet<City> Cities { get; set; }
    public DbSet<District> Districts { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<PaymentProductDetail> PaymentProductDetails { get; set; }
    public DbSet<PaymentUserDetail> PaymentUserDetails { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Reply> Replies { get; set; }

    public DbSet<Address> Addresses { get; set; }

    public DbSet<UserPermision> UserPermisions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      modelBuilder.Entity<Product>().ToTable("Product").HasIndex(p => p.Slug).IsUnique();
      modelBuilder.Entity<Cart>().ToTable("Cart");
      modelBuilder.Entity<CartItem>().ToTable("CartItem");
      modelBuilder.Entity<ProductImage>().ToTable("ProductImage");
      modelBuilder.Entity<Category>().ToTable("Category");
      modelBuilder.Entity<City>().ToTable("City");
      modelBuilder.Entity<District>().ToTable("District");
      modelBuilder.Entity<Payment>().ToTable("Payment");
      modelBuilder.Entity<PaymentUserDetail>().ToTable("PaymentUserDetail");
      modelBuilder.Entity<PaymentProductDetail>().ToTable("PaymentProductDetail");
      modelBuilder.Entity<Review>().ToTable("Review");
      modelBuilder.Entity<Reply>().ToTable("Reply");
      modelBuilder.Entity<Address>().ToTable("Address");
      modelBuilder.Entity<UserPermision>().ToTable("UserPermision");
    }


    public DbSet<Chim_En_DOTNET.Models.ApplicationUser> ApplicationUser { get; set; }
  }
}
