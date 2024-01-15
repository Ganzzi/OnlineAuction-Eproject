using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V1 : Migration
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
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    Price = table.Column<float>(type: "real", nullable: false),
                    ImgUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTable", x => x.ItemId);
                    table.ForeignKey(
                        name: "FK_ItemTable_Usertable_UserId",
                        column: x => x.UserId,
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
                name: "AcutionHistory",
                columns: table => new
                {
                    AcutionHistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    startDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    endDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WinningBid = table.Column<float>(type: "real", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcutionHistory", x => x.AcutionHistoryId);
                    table.ForeignKey(
                        name: "FK_AcutionHistory_ItemTable_ItemId",
                        column: x => x.ItemId,
                        principalTable: "ItemTable",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AcutionHistory_Usertable_UserId",
                        column: x => x.UserId,
                        principalTable: "Usertable",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BidTable",
                columns: table => new
                {
                    BidId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    BidAmout = table.Column<float>(type: "real", nullable: false),
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
                    CategoryId = table.Column<int>(type: "int", nullable: false),
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
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Rate = table.Column<float>(type: "real", nullable: false),
                    RatingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                        name: "FK_Rating_Usertable_UserId",
                        column: x => x.UserId,
                        principalTable: "Usertable",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CategoryTable",
                columns: new[] { "CategoryId", "CategoryName", "Created", "Description", "LastModified" },
                values: new object[,]
                {
                    { 1, "Category 1", null, "Description for Category 1", null },
                    { 2, "Category 2", null, "Description for Category 2", null }
                });

            migrationBuilder.InsertData(
                table: "Usertable",
                columns: new[] { "UserId", "Created", "Email", "LastModified", "Name", "Password", "Role" },
                values: new object[,]
                {
                    { 1, null, "batman123", null, "batman", "123", "User" },
                    { 2, null, "ironman123", null, "ironman", "123", "User" },
                    { 3, null, "admin123", null, "admin", "123", "Admin" }
                });

            migrationBuilder.InsertData(
                table: "ItemTable",
                columns: new[] { "ItemId", "Created", "Description", "ImgUrl", "LastModified", "Price", "Title", "UserId" },
                values: new object[,]
                {
                    { 1, null, "Description for Item 1", "url_to_image_1", null, 1000f, "Item 1", 1 },
                    { 2, null, "Description for Item 2", "url_to_image_2", null, 2000f, "Item 2", 1 }
                });

            migrationBuilder.InsertData(
                table: "BidTable",
                columns: new[] { "BidId", "BidAmout", "BidDate", "Created", "ItemId", "LastModified", "UserId" },
                values: new object[,]
                {
                    { 1, 100f, new DateTime(2024, 1, 15, 12, 21, 36, 669, DateTimeKind.Local).AddTicks(8523), null, 1, null, 1 },
                    { 2, 200f, new DateTime(2024, 1, 15, 12, 21, 36, 669, DateTimeKind.Local).AddTicks(8551), null, 2, null, 2 }
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
                columns: new[] { "RatingId", "Created", "ItemId", "LastModified", "Rate", "RatingDate", "UserId" },
                values: new object[,]
                {
                    { 1, null, 1, null, 4.5f, new DateTime(2024, 1, 15, 12, 21, 36, 669, DateTimeKind.Local).AddTicks(8618), 1 },
                    { 2, null, 2, null, 4f, new DateTime(2024, 1, 15, 12, 21, 36, 669, DateTimeKind.Local).AddTicks(8620), 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcutionHistory_ItemId",
                table: "AcutionHistory",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_AcutionHistory_UserId",
                table: "AcutionHistory",
                column: "UserId");

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
                name: "IX_ItemTable_UserId",
                table: "ItemTable",
                column: "UserId");

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
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Rating_UserId",
                table: "Rating",
                column: "UserId");

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
                name: "AcutionHistory");

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
