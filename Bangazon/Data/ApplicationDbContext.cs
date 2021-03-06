﻿
using System;
using System.Collections.Generic;
using System.Text;
using Bangazon.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bangazon.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductType> ProductType { get; set; }
        public DbSet<PaymentType> PaymentType { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderProduct> OrderProduct { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            modelBuilder.Entity<Order>()
                .Property(b => b.DateCreated)
                .HasDefaultValueSql("GETDATE()");

            // Restrict deletion of related order when OrderProducts entry is removed
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderProducts)
                .WithOne(l => l.Order)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .Property(b => b.DateCreated)
                .HasDefaultValueSql("GETDATE()");

            // Restrict deletion of related product when OrderProducts entry is removed
            modelBuilder.Entity<Product>()
                .HasMany(o => o.OrderProducts)
                .WithOne(l => l.Product)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PaymentType>()
                .Property(b => b.DateCreated)
                .HasDefaultValueSql("GETDATE()");


            ApplicationUser user = new ApplicationUser
            {
                FirstName = "admin",
                LastName = "admin",
                StreetAddress = "123 Infinity Way",
                UserName = "admin@admin.com",
                NormalizedUserName = "ADMIN@ADMIN.COM",
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };
            var passwordHash = new PasswordHasher<ApplicationUser>();
            user.PasswordHash = passwordHash.HashPassword(user, "Admin8*");
            modelBuilder.Entity<ApplicationUser>().HasData(user);


            modelBuilder.Entity<PaymentType>().HasData(
                new PaymentType()
                {
                    PaymentTypeId = 1,
                    UserId = user.Id,
                    Description = "American Express",
                    AccountNumber = "86753095551212"
                },
                new PaymentType()
                {
                    PaymentTypeId = 2,
                    UserId = user.Id,
                    Description = "Discover",
                    AccountNumber = "4102948572991"
                }
            );


            modelBuilder.Entity<ProductType>().HasData(
           new ProductType()
           {
               ProductTypeId = 1,
               Label = "CLothing",
               Quantity = 100,
           },
           new ProductType()
           {
               ProductTypeId = 2,
               Label = "Accessories",
               Quantity = 100,
           }
           );

            modelBuilder.Entity<Product>().HasData(
               new Product()
               {
                   ProductId = 1,
                   Description = "Soft Scarf",
                   Title = "Scarf",
                   Price = 15.00,
                   Quantity = 40,
                   City = "Seattle",
                   ProductTypeId = 1,
                   UserId = user.Id
               },
               new Product()
               {
                   ProductId = 2,
                   Description = "So Fluffy",
                   Title = "Fluffy Socks",
                   Price = 5.00,
                   Quantity = 30,
                   City = "Portland",
                   ProductTypeId = 1,
                   UserId = user.Id

               },
               new Product()
               {
                   ProductId = 3,
                   Description = "ARRGGG",
                   Title = "Pirate Hat",
                   Price = 25.00,
                   Quantity = 50,
                   City = "Nashville",
                   ProductTypeId = 1,
                   UserId = user.Id

               }
           );
            modelBuilder.Entity<Order>().HasData(
               new Order()
               {
                   OrderId = 1,
                   UserId = user.Id,
                   PaymentTypeId = 1
               },
               new Order()
               {
                   OrderId = 2,
                   UserId = user.Id,
                   PaymentTypeId = 2
               },
               new Order()
               {
                   OrderId = 3,
                   UserId = user.Id,
                   PaymentTypeId = null
               }

          );

            modelBuilder.Entity<OrderProduct>().HasData(
           new OrderProduct()
           {
               OrderProductId = 1,
               OrderId = 1,
               ProductId = 2
           },
           new OrderProduct()
           {
               OrderProductId = 2,
               OrderId = 2,
               ProductId = 1
           }
       );




        }
    }
}