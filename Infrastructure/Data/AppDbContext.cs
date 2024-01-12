﻿using DomainLayer.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<User> Usertable { get; set; }
        public DbSet<Bid> BidTable { get; set; }
        public DbSet<AcutionHistory> AcutionHistory { get; set; }
        public DbSet<Category> CategoryTable { get; set; }
        public DbSet<Item> ItemTable { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<Rating> Rating { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<CategoryItem> CategoryItem { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    Name = "batman",
                    Password = "123",
                    Email = "batman123",
                    Role = "User",
                },
                new User
                {
                    UserId = 2,
                    Name = "ironman",
                    Password = "123",
                    Email = "ironman123",
                    Role = "User",
                },
                new User
                {
                    UserId = 3,
                    Name = "admin",
                    Password = "123",
                    Email = "admin123",
                    Role = "Admin",
                }
            );

            modelBuilder.Entity<Bid>().HasData(
                new Bid
                {
                    BidId = 1,
                    UserId = 1,
                    ItemId = 1,
                    BidAmout = 100,
                    BidDate = DateTime.Now
                },
                new Bid
                {
                    BidId = 2,
                    UserId = 2,
                    ItemId = 2,
                    BidAmout = 200,
                    BidDate = DateTime.Now
                }
            );

            modelBuilder.Entity<Item>().HasData(
                new Item
                {
                    ItemId = 1,
                    Title = "Item 1",
                    Description = "Description for Item 1",
                    Price = 1000,
                    ImgUrl = "url_to_image_1"
                },
                new Item
                {
                    ItemId = 2,
                    Title = "Item 2",
                    Description = "Description for Item 2",
                    Price = 2000,
                    ImgUrl = "url_to_image_2"
                }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Category_Id = 1,
                    Category_Name = "Category 1",
                    Description = "Description for Category 1"
                },
                new Category
                {
                    Category_Id = 2,
                    Category_Name = "Category 2",
                    Description = "Description for Category 2"
                }
            );

            modelBuilder.Entity<Rating>().HasData(
                new Rating
                {
                    RatingId = 1,
                    ItemId = 1,
                    UserId = 1,
                    Rate = 4.5f,
                    Rating_date = DateTime.Now
                },
                new Rating
                {
                    RatingId = 2,
                    ItemId = 2,
                    UserId = 2,
                    Rate = 4.0f,
                    Rating_date = DateTime.Now
                }
            );

            modelBuilder.Entity<CategoryItem>().HasData(
                new CategoryItem
                {
                    Id = 1,
                    ItemId = 1,
                    categoryId = 1
                },
                new CategoryItem
                {
                    Id = 2,
                    ItemId = 2,
                    categoryId = 2
                }
            );

            // Configure the relationship between Category and CategoryItem
            modelBuilder.Entity<Category>()
                .HasMany(c => c.CategoriesItem)
                .WithOne(ci => ci.category)
                .HasForeignKey(ci => ci.categoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure the relationship between Item and CategoryItem
            modelBuilder.Entity<Item>()
                .HasMany(i => i.CategoriesItem)
                .WithOne(ci => ci.item)
                .HasForeignKey(ci => ci.ItemId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
    public class BloggingContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer("Data Source=LInh;Initial Catalog=AcutionDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
