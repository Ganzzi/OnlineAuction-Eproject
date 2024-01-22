﻿// <auto-generated />
using System;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240122125006_V0")]
    partial class V0
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DomainLayer.Entities.Models.AuctionHistory", b =>
                {
                    b.Property<int>("AuctionHistoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AuctionHistoryId"));

                    b.Property<DateTime?>("Created")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<int>("WinnerId")
                        .HasColumnType("int");

                    b.Property<float>("WinningBid")
                        .HasColumnType("real");

                    b.HasKey("AuctionHistoryId");

                    b.HasIndex("ItemId")
                        .IsUnique();

                    b.HasIndex("WinnerId");

                    b.ToTable("AuctionHistory");
                });

            modelBuilder.Entity("DomainLayer.Entities.Models.Bid", b =>
                {
                    b.Property<int>("BidId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BidId"));

                    b.Property<float>("BidAmout")
                        .HasColumnType("real");

                    b.Property<DateTime>("BidDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Created")
                        .HasColumnType("datetime2");

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("BidId");

                    b.HasIndex("ItemId");

                    b.HasIndex("UserId");

                    b.ToTable("BidTable");

                    b.HasData(
                        new
                        {
                            BidId = 1,
                            BidAmout = 100f,
                            BidDate = new DateTime(2024, 1, 22, 19, 50, 6, 761, DateTimeKind.Local).AddTicks(6314),
                            ItemId = 1,
                            UserId = 1
                        },
                        new
                        {
                            BidId = 2,
                            BidAmout = 200f,
                            BidDate = new DateTime(2024, 1, 22, 19, 50, 6, 761, DateTimeKind.Local).AddTicks(6322),
                            ItemId = 2,
                            UserId = 2
                        });
                });

            modelBuilder.Entity("DomainLayer.Entities.Models.Category", b =>
                {
                    b.Property<int?>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("CategoryId"));

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.HasKey("CategoryId");

                    b.ToTable("CategoryTable");

                    b.HasData(
                        new
                        {
                            CategoryId = 1,
                            CategoryName = "Category 1",
                            Description = "Description for Category 1"
                        },
                        new
                        {
                            CategoryId = 2,
                            CategoryName = "Category 2",
                            Description = "Description for Category 2"
                        });
                });

            modelBuilder.Entity("DomainLayer.Entities.Models.CategoryItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Created")
                        .HasColumnType("datetime2");

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("ItemId");

                    b.ToTable("CategoryItem");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CategoryId = 1,
                            ItemId = 1
                        },
                        new
                        {
                            Id = 2,
                            CategoryId = 2,
                            ItemId = 2
                        });
                });

            modelBuilder.Entity("DomainLayer.Entities.Models.Item", b =>
                {
                    b.Property<int>("ItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ItemId"));

                    b.Property<DateTime?>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("IncreasingAmount")
                        .HasColumnType("real");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<float?>("ReservePrice")
                        .HasColumnType("real");

                    b.Property<int>("SellerId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<float>("StartingPrice")
                        .HasColumnType("real");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ItemId");

                    b.HasIndex("SellerId");

                    b.ToTable("ItemTable");

                    b.HasData(
                        new
                        {
                            ItemId = 1,
                            Description = "Description for Item 1",
                            EndDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Image = "url_to_image_1",
                            IncreasingAmount = 100f,
                            SellerId = 1,
                            StartDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            StartingPrice = 1000f,
                            Title = "Item 1"
                        },
                        new
                        {
                            ItemId = 2,
                            Description = "Description for Item 2",
                            EndDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Image = "url_to_image_2",
                            IncreasingAmount = 100f,
                            SellerId = 1,
                            StartDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            StartingPrice = 2000f,
                            Title = "Item 2"
                        });
                });

            modelBuilder.Entity("DomainLayer.Entities.Models.Notification", b =>
                {
                    b.Property<int>("NotificationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NotificationId"));

                    b.Property<DateTime?>("Created")
                        .HasColumnType("datetime2");

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("NotificationContent")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("NotificationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("NotificationId");

                    b.HasIndex("ItemId");

                    b.HasIndex("UserId");

                    b.ToTable("Notification");
                });

            modelBuilder.Entity("DomainLayer.Entities.Models.Rating", b =>
                {
                    b.Property<int>("RatingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RatingId"));

                    b.Property<DateTime?>("Created")
                        .HasColumnType("datetime2");

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<float>("Rate")
                        .HasColumnType("real");

                    b.Property<int>("RatedUserId")
                        .HasColumnType("int");

                    b.Property<int>("RaterId")
                        .HasColumnType("int");

                    b.Property<DateTime>("RatingDate")
                        .HasColumnType("datetime2");

                    b.HasKey("RatingId");

                    b.HasIndex("ItemId")
                        .IsUnique();

                    b.HasIndex("RatedUserId");

                    b.HasIndex("RaterId");

                    b.ToTable("Rating");

                    b.HasData(
                        new
                        {
                            RatingId = 1,
                            ItemId = 1,
                            Rate = 4.5f,
                            RatedUserId = 2,
                            RaterId = 1,
                            RatingDate = new DateTime(2024, 1, 22, 19, 50, 6, 761, DateTimeKind.Local).AddTicks(6357)
                        },
                        new
                        {
                            RatingId = 2,
                            ItemId = 2,
                            Rate = 4f,
                            RatedUserId = 2,
                            RaterId = 2,
                            RatingDate = new DateTime(2024, 1, 22, 19, 50, 6, 761, DateTimeKind.Local).AddTicks(6360)
                        });
                });

            modelBuilder.Entity("DomainLayer.Entities.Models.RefreshToken", b =>
                {
                    b.Property<int>("RefreshTokeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RefreshTokeId"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("RefreshTokeId");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasFilter("[UserId] IS NOT NULL");

                    b.ToTable("RefreshToken");
                });

            modelBuilder.Entity("DomainLayer.Entities.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Avatar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Usertable");

                    b.HasData(
                        new
                        {
                            UserId = 1,
                            Email = "batman123",
                            Name = "batman",
                            Password = "123",
                            Role = "User"
                        },
                        new
                        {
                            UserId = 2,
                            Email = "ironman123",
                            Name = "ironman",
                            Password = "123",
                            Role = "User"
                        },
                        new
                        {
                            UserId = 3,
                            Email = "admin123",
                            Name = "admin",
                            Password = "123",
                            Role = "Admin"
                        });
                });

            modelBuilder.Entity("DomainLayer.Entities.Models.AuctionHistory", b =>
                {
                    b.HasOne("DomainLayer.Entities.Models.Item", "Item")
                        .WithOne("AuctionHistory")
                        .HasForeignKey("DomainLayer.Entities.Models.AuctionHistory", "ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DomainLayer.Entities.Models.User", "Winner")
                        .WithMany("AuctionHistories")
                        .HasForeignKey("WinnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");

                    b.Navigation("Winner");
                });

            modelBuilder.Entity("DomainLayer.Entities.Models.Bid", b =>
                {
                    b.HasOne("DomainLayer.Entities.Models.Item", "Item")
                        .WithMany("Bids")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DomainLayer.Entities.Models.User", "User")
                        .WithMany("Bids")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DomainLayer.Entities.Models.CategoryItem", b =>
                {
                    b.HasOne("DomainLayer.Entities.Models.Category", "Category")
                        .WithMany("CategoryItems")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DomainLayer.Entities.Models.Item", "Item")
                        .WithMany("CategoryItems")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Item");
                });

            modelBuilder.Entity("DomainLayer.Entities.Models.Item", b =>
                {
                    b.HasOne("DomainLayer.Entities.Models.User", "Seller")
                        .WithMany("SoldItems")
                        .HasForeignKey("SellerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Seller");
                });

            modelBuilder.Entity("DomainLayer.Entities.Models.Notification", b =>
                {
                    b.HasOne("DomainLayer.Entities.Models.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DomainLayer.Entities.Models.User", "User")
                        .WithMany("Notifications")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DomainLayer.Entities.Models.Rating", b =>
                {
                    b.HasOne("DomainLayer.Entities.Models.Item", "Item")
                        .WithOne("Rating")
                        .HasForeignKey("DomainLayer.Entities.Models.Rating", "ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DomainLayer.Entities.Models.User", "RatedUser")
                        .WithMany("BeingRateds")
                        .HasForeignKey("RatedUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DomainLayer.Entities.Models.User", "Rater")
                        .WithMany("Ratings")
                        .HasForeignKey("RaterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");

                    b.Navigation("RatedUser");

                    b.Navigation("Rater");
                });

            modelBuilder.Entity("DomainLayer.Entities.Models.RefreshToken", b =>
                {
                    b.HasOne("DomainLayer.Entities.Models.User", "User")
                        .WithOne("RefreshToken")
                        .HasForeignKey("DomainLayer.Entities.Models.RefreshToken", "UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DomainLayer.Entities.Models.Category", b =>
                {
                    b.Navigation("CategoryItems");
                });

            modelBuilder.Entity("DomainLayer.Entities.Models.Item", b =>
                {
                    b.Navigation("AuctionHistory");

                    b.Navigation("Bids");

                    b.Navigation("CategoryItems");

                    b.Navigation("Rating");
                });

            modelBuilder.Entity("DomainLayer.Entities.Models.User", b =>
                {
                    b.Navigation("AuctionHistories");

                    b.Navigation("BeingRateds");

                    b.Navigation("Bids");

                    b.Navigation("Notifications");

                    b.Navigation("Ratings");

                    b.Navigation("RefreshToken");

                    b.Navigation("SoldItems");
                });
#pragma warning restore 612, 618
        }
    }
}
