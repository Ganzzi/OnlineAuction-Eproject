using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoryTable",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryTable", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Usertable",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tokenResetPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResetExpire = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usertable", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "ItemTable",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartingPrice = table.Column<float>(type: "real", nullable: false),
                    IncreasingAmount = table.Column<float>(type: "real", nullable: false),
                    ReservePrice = table.Column<float>(type: "real", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Document = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SellerId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTable", x => x.ItemId);
                    table.ForeignKey(
                        name: "FK_ItemTable_Usertable_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Usertable",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    RefreshTokeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.RefreshTokeId);
                    table.ForeignKey(
                        name: "FK_RefreshToken_Usertable_UserId",
                        column: x => x.UserId,
                        principalTable: "Usertable",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "AuctionHistory",
                columns: table => new
                {
                    AuctionHistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    WinnerId = table.Column<int>(type: "int", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WinningBid = table.Column<float>(type: "real", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuctionHistory", x => x.AuctionHistoryId);
                    table.ForeignKey(
                        name: "FK_AuctionHistory_ItemTable_ItemId",
                        column: x => x.ItemId,
                        principalTable: "ItemTable",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuctionHistory_Usertable_WinnerId",
                        column: x => x.WinnerId,
                        principalTable: "Usertable",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "BidTable",
                columns: table => new
                {
                    BidId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    BidAmount = table.Column<float>(type: "real", nullable: false),
                    BidDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BidTable", x => x.BidId);
                    table.ForeignKey(
                        name: "FK_BidTable_ItemTable_ItemId",
                        column: x => x.ItemId,
                        principalTable: "ItemTable",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BidTable_Usertable_UserId",
                        column: x => x.UserId,
                        principalTable: "Usertable",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoryItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryItem_CategoryTable_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "CategoryTable",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryItem_ItemTable_ItemId",
                        column: x => x.ItemId,
                        principalTable: "ItemTable",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    NotificationContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NotificationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_Notification_ItemTable_ItemId",
                        column: x => x.ItemId,
                        principalTable: "ItemTable",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notification_Usertable_UserId",
                        column: x => x.UserId,
                        principalTable: "Usertable",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rating",
                columns: table => new
                {
                    RatingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rate = table.Column<float>(type: "real", nullable: false),
                    RatingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    RaterId = table.Column<int>(type: "int", nullable: false),
                    RatedUserId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rating", x => x.RatingId);
                    table.ForeignKey(
                        name: "FK_Rating_ItemTable_ItemId",
                        column: x => x.ItemId,
                        principalTable: "ItemTable",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rating_Usertable_RatedUserId",
                        column: x => x.RatedUserId,
                        principalTable: "Usertable",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rating_Usertable_RaterId",
                        column: x => x.RaterId,
                        principalTable: "Usertable",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "CategoryTable",
                columns: new[] { "CategoryId", "CategoryName", "Created", "Description", "LastModified" },
                values: new object[,]
                {
                    { 1, "Sports", null, "Get the latest scoop on your favorite sports teams and players, plus expert analysis and commentary on the biggest games and events", null },
                    { 2, "Health", null, "Stay healthy and informed with our comprehensive coverage of health and wellness", null }
                });

            migrationBuilder.InsertData(
                table: "Usertable",
                columns: new[] { "UserId", "Avatar", "Created", "Email", "LastModified", "Name", "Password", "ResetExpire", "Role", "tokenResetPassword" },
                values: new object[,]
                {
                    { 1, "https://res.cloudinary.com/dcxzqj0ta/image/upload/v1705895402/o5o4yqt8puuurevqlwmp.png", null, "batman123@gmail.com", null, "batman", "B6-D6-6B-8E-89-F4-6D-29-FC-74-80-55-2E-5C-D3-47-47-C0-C7-A3-01-73-64-32-9E-92-99-C3-F1-DF-01-D3", null, "User", null },
                    { 2, "https://res.cloudinary.com/dcxzqj0ta/image/upload/v1705895402/o5o4yqt8puuurevqlwmp.png", null, "ironman123@gmail.com", null, "ironman", "9A-44-C4-72-F3-19-B3-74-D5-98-C1-06-61-9B-6B-02-A7-C9-57-7B-C7-82-6E-80-61-F2-88-B7-2A-38-8C-0E", null, "User", null },
                    { 3, "https://res.cloudinary.com/dcxzqj0ta/image/upload/v1705895402/o5o4yqt8puuurevqlwmp.png", null, "admin123@gmail.com", null, "admin", "60-FE-74-40-6E-7F-35-3E-D9-79-F3-50-F2-FB-B6-A2-E8-69-0A-5F-A7-D1-B0-C3-29-83-D1-D8-B3-F9-5F-67", null, "Admin", null }
                });

            migrationBuilder.InsertData(
                table: "ItemTable",
                columns: new[] { "ItemId", "Created", "Description", "Document", "EndDate", "Image", "IncreasingAmount", "LastModified", "ReservePrice", "SellerId", "StartDate", "StartingPrice", "Title" },
                values: new object[,]
                {
                    { 1, null, "Description for Item 1", "Online Auction.doc", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://res.cloudinary.com/dcxzqj0ta/image/upload/v1705895402/o5o4yqt8puuurevqlwmp.png", 2f, null, 16f, 1, new DateTime(2024, 2, 2, 15, 0, 53, 350, DateTimeKind.Local).AddTicks(4876), 8f, "Item 1" },
                    { 2, null, "Description for Item 2", "Online Auction.doc", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://res.cloudinary.com/dcxzqj0ta/image/upload/v1705895402/o5o4yqt8puuurevqlwmp.png", 2f, null, 20f, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 10f, "Item 2" }
                });

            migrationBuilder.InsertData(
                table: "AuctionHistory",
                columns: new[] { "AuctionHistoryId", "Created", "EndDate", "ItemId", "LastModified", "WinnerId", "WinningBid" },
                values: new object[,]
                {
                    { 1, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, null, 199f },
                    { 2, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, null, 199f }
                });

            migrationBuilder.InsertData(
                table: "BidTable",
                columns: new[] { "BidId", "BidAmount", "BidDate", "Created", "ItemId", "LastModified", "UserId" },
                values: new object[,]
                {
                    { 1, 10f, new DateTime(2024, 2, 2, 15, 0, 53, 350, DateTimeKind.Local).AddTicks(4820), null, 2, null, 1 },
                    { 2, 12f, new DateTime(2024, 2, 2, 15, 0, 53, 350, DateTimeKind.Local).AddTicks(4852), null, 1, null, 2 }
                });

            migrationBuilder.InsertData(
                table: "CategoryItem",
                columns: new[] { "Id", "CategoryId", "Created", "ItemId", "LastModified" },
                values: new object[,]
                {
                    { 1, 1, null, 1, null },
                    { 2, 2, null, 2, null }
                });

            migrationBuilder.InsertData(
                table: "Rating",
                columns: new[] { "RatingId", "Created", "ItemId", "LastModified", "Rate", "RatedUserId", "RaterId", "RatingDate" },
                values: new object[,]
                {
                    { 1, null, 1, null, 4.5f, 2, 1, new DateTime(2024, 2, 2, 15, 0, 53, 350, DateTimeKind.Local).AddTicks(4936) },
                    { 2, null, 2, null, 4f, 1, 2, new DateTime(2024, 2, 2, 15, 0, 53, 350, DateTimeKind.Local).AddTicks(4939) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuctionHistory_ItemId",
                table: "AuctionHistory",
                column: "ItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuctionHistory_WinnerId",
                table: "AuctionHistory",
                column: "WinnerId");

            migrationBuilder.CreateIndex(
                name: "IX_BidTable_ItemId",
                table: "BidTable",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_BidTable_UserId",
                table: "BidTable",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryItem_CategoryId",
                table: "CategoryItem",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryItem_ItemId",
                table: "CategoryItem",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTable_SellerId",
                table: "ItemTable",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_ItemId",
                table: "Notification",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_UserId",
                table: "Notification",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Rating_ItemId",
                table: "Rating",
                column: "ItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rating_RatedUserId",
                table: "Rating",
                column: "RatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Rating_RaterId",
                table: "Rating",
                column: "RaterId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_UserId",
                table: "RefreshToken",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuctionHistory");

            migrationBuilder.DropTable(
                name: "BidTable");

            migrationBuilder.DropTable(
                name: "CategoryItem");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "Rating");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "CategoryTable");

            migrationBuilder.DropTable(
                name: "ItemTable");

            migrationBuilder.DropTable(
                name: "Usertable");
        }
    }
}
