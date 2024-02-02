using DomainLayer.Entities.Models;
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
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
        public DbSet<User> Usertable { get; set; }
        public DbSet<Bid> BidTable { get; set; }
        public DbSet<AuctionHistory> AuctionHistory { get; set; }
        public DbSet<Category> CategoryTable { get; set; }
        public DbSet<Item> ItemTable { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<Rating> Rating { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<CategoryItem> CategoryItem { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // modelBuilder.Entity<User>().HasData(
            //     new User
            //     {
            //         UserId = 1,
            //         Name = "batman",
            //         Password = "B6-D6-6B-8E-89-F4-6D-29-FC-74-80-55-2E-5C-D3-47-47-C0-C7-A3-01-73-64-32-9E-92-99-C3-F1-DF-01-D3",
            //         Email = "batman123@gmail.com",
            //         Role = "User",
            //         Avatar = "https://res.cloudinary.com/dcxzqj0ta/image/upload/v1705895402/o5o4yqt8puuurevqlwmp.png"
            //     },
            //     new User
            //     {
            //         UserId = 2,
            //         Name = "ironman",
            //         Password = "9A-44-C4-72-F3-19-B3-74-D5-98-C1-06-61-9B-6B-02-A7-C9-57-7B-C7-82-6E-80-61-F2-88-B7-2A-38-8C-0E",
            //         Email = "ironman123@gmail.com",
            //         Role = "User",
            //         Avatar = "https://res.cloudinary.com/dcxzqj0ta/image/upload/v1705895402/o5o4yqt8puuurevqlwmp.png"
            //     },
            //     new User
            //     {
            //         UserId = 3,
            //         Name = "admin",
            //         Password = "60-FE-74-40-6E-7F-35-3E-D9-79-F3-50-F2-FB-B6-A2-E8-69-0A-5F-A7-D1-B0-C3-29-83-D1-D8-B3-F9-5F-67",
            //         Email = "admin123@gmail.com",
            //         Role = "Admin",
            //         Avatar = "https://res.cloudinary.com/dcxzqj0ta/image/upload/v1705895402/o5o4yqt8puuurevqlwmp.png"
            //     }
            // );

            // modelBuilder.Entity<Bid>().HasData(
            //     new Bid
            //     {
            //         BidId = 1,
            //         UserId = 1,
            //         ItemId = 2,
            //         BidAmount = 10,
            //         BidDate = DateTime.Now
            //     },
            //     new Bid
            //     {
            //         BidId = 2,
            //         UserId = 2,
            //         ItemId = 1,
            //         BidAmount = 12,
            //         BidDate = DateTime.Now
            //     }
            // );

            // modelBuilder.Entity<Item>().HasData(
            //           new Item
            //           {
            //               ItemId = 1,
            //               Title = "Item 1",
            //               Description = "Description for Item 1",
            //               StartingPrice = 8,
            //               IncreasingAmount = 2,
            //               ReservePrice = 16,
            //               SellerId = 1,
            //               StartDate = DateTime.Now,
            //               Image = "https://res.cloudinary.com/dcxzqj0ta/image/upload/v1705895402/o5o4yqt8puuurevqlwmp.png",
            //               Document = "Online Auction.doc"
            //           },
            //           new Item
            //           {
            //               ItemId = 2,
            //               Title = "Item 2",
            //               Description = "Description for Item 2",
            //               StartingPrice = 10,
            //               IncreasingAmount = 2,
            //               ReservePrice = 20,
            //               SellerId = 1,
            //               Image = "https://res.cloudinary.com/dcxzqj0ta/image/upload/v1705895402/o5o4yqt8puuurevqlwmp.png",
            //               Document = "Online Auction.doc"
            //           }
            //       );

            // modelBuilder.Entity<Category>().HasData(
            //     new Category
            //     {
            //         CategoryId = 1,
            //         CategoryName = "Sports",
            //         Description = "Get the latest scoop on your favorite sports teams and players, plus expert analysis and commentary on the biggest games and events"
            //     },
            //     new Category
            //     {
            //         CategoryId = 2,
            //         CategoryName = "Health",
            //         Description = "Stay healthy and informed with our comprehensive coverage of health and wellness"
            //     }
            // );

            // modelBuilder.Entity<AuctionHistory>().HasData(
            //           new AuctionHistory
            //           {
            //               AuctionHistoryId = 1,
            //               ItemId = 1,
            //               EndDate = new DateTime(),
            //               WinningBid = 199
            //             },
            //           new AuctionHistory
            //           {
            //               AuctionHistoryId = 2,
            //               ItemId = 2,
            //               EndDate = new DateTime(),
            //               WinningBid = 199
            //             }
            //       );

            // modelBuilder.Entity<Rating>().HasData(
            //     new Rating
            //     {
            //         RatingId = 1,
            //         ItemId = 1,
            //         RaterId = 1,
            //         RatedUserId = 2,
            //         Rate = 4.5f,
            //         RatingDate = DateTime.Now
            //     },
            //     new Rating
            //     {
            //         RatingId = 2,
            //         ItemId = 2,
            //         RaterId = 2,
            //         RatedUserId = 1,
            //         Rate = 4.0f,
            //         RatingDate = DateTime.Now
            //     }
            // );

            // modelBuilder.Entity<CategoryItem>().HasData(
            //     new CategoryItem
            //     {
            //         Id = 1,
            //         ItemId = 1,
            //         CategoryId = 1
            //     },
            //     new CategoryItem
            //     {
            //         Id = 2,
            //         ItemId = 2,
            //         CategoryId = 2
            //     }
            // );

            // Configure the relationship between Category and CategoryItem
            modelBuilder.Entity<Category>()
                .HasMany(c => c.CategoryItems)
                .WithOne(ci => ci.Category)
                .HasForeignKey(ci => ci.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure the relationship between Item and CategoryItem
            modelBuilder.Entity<Item>()
                .HasMany(i => i.CategoryItems)
                .WithOne(ci => ci.Item)
                .HasForeignKey(ci => ci.ItemId)
                .OnDelete(DeleteBehavior.Cascade);
            // Configure the relationship between Item and User
            modelBuilder.Entity<User>()
                .HasMany(u => u.SoldItems)
                .WithOne(i => i.Seller)
                .HasForeignKey(i => i.SellerId)
                .OnDelete(DeleteBehavior.Restrict);
            // Configure the relationship between Rating and User(Rater)
            modelBuilder.Entity<User>()
                .HasMany(x => x.Ratings)
                .WithOne(x => x.Rater)
                .HasForeignKey(c => c.RaterId)
                .OnDelete(DeleteBehavior.Restrict);
            // Configure the relationship between Rating and User(RateUser)
            modelBuilder.Entity<User>()
              .HasMany(x => x.BeingRateds)
              .WithOne(x => x.RatedUser)
              .HasForeignKey(c => c.RatedUserId)
              .OnDelete(DeleteBehavior.Restrict);
        }
    }

    /// <summary>
    /// For development only (migrations and database update)
    /// </summary>
    public class BloggingContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(
                       "Server=localhost;Database=AuctionOnline;User ID=sa;Password=StrongPassword123@;TrustServerCertificate=true;"
                    );

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
