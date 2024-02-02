using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewMigration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AuctionHistory",
                keyColumn: "AuctionHistoryId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AuctionHistory",
                keyColumn: "AuctionHistoryId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "BidTable",
                keyColumn: "BidId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "BidTable",
                keyColumn: "BidId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CategoryItem",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CategoryItem",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Rating",
                keyColumn: "RatingId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Rating",
                keyColumn: "RatingId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Usertable",
                keyColumn: "UserId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "CategoryTable",
                keyColumn: "CategoryId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CategoryTable",
                keyColumn: "CategoryId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ItemTable",
                keyColumn: "ItemId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ItemTable",
                keyColumn: "ItemId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Usertable",
                keyColumn: "UserId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Usertable",
                keyColumn: "UserId",
                keyValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
