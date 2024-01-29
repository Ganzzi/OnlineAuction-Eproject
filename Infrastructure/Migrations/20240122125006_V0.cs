using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rating_Usertable_RatedUserId",
                table: "Rating");

            migrationBuilder.AlterColumn<int>(
                name: "RatedUserId",
                table: "Rating",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "BidTable",
                keyColumn: "BidId",
                keyValue: 1,
                column: "BidDate",
                value: new DateTime(2024, 1, 22, 19, 50, 6, 761, DateTimeKind.Local).AddTicks(6314));

            migrationBuilder.UpdateData(
                table: "BidTable",
                keyColumn: "BidId",
                keyValue: 2,
                column: "BidDate",
                value: new DateTime(2024, 1, 22, 19, 50, 6, 761, DateTimeKind.Local).AddTicks(6322));

            migrationBuilder.UpdateData(
                table: "Rating",
                keyColumn: "RatingId",
                keyValue: 1,
                column: "RatingDate",
                value: new DateTime(2024, 1, 22, 19, 50, 6, 761, DateTimeKind.Local).AddTicks(6357));

            migrationBuilder.UpdateData(
                table: "Rating",
                keyColumn: "RatingId",
                keyValue: 2,
                column: "RatingDate",
                value: new DateTime(2024, 1, 22, 19, 50, 6, 761, DateTimeKind.Local).AddTicks(6360));

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_Usertable_RatedUserId",
                table: "Rating",
                column: "RatedUserId",
                principalTable: "Usertable",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rating_Usertable_RatedUserId",
                table: "Rating");

            migrationBuilder.AlterColumn<int>(
                name: "RatedUserId",
                table: "Rating",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "BidTable",
                keyColumn: "BidId",
                keyValue: 1,
                column: "BidDate",
                value: new DateTime(2024, 1, 18, 18, 18, 33, 534, DateTimeKind.Local).AddTicks(3487));

            migrationBuilder.UpdateData(
                table: "BidTable",
                keyColumn: "BidId",
                keyValue: 2,
                column: "BidDate",
                value: new DateTime(2024, 1, 18, 18, 18, 33, 534, DateTimeKind.Local).AddTicks(3512));

            migrationBuilder.UpdateData(
                table: "Rating",
                keyColumn: "RatingId",
                keyValue: 1,
                column: "RatingDate",
                value: new DateTime(2024, 1, 18, 18, 18, 33, 534, DateTimeKind.Local).AddTicks(3574));

            migrationBuilder.UpdateData(
                table: "Rating",
                keyColumn: "RatingId",
                keyValue: 2,
                column: "RatingDate",
                value: new DateTime(2024, 1, 18, 18, 18, 33, 534, DateTimeKind.Local).AddTicks(3576));

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_Usertable_RatedUserId",
                table: "Rating",
                column: "RatedUserId",
                principalTable: "Usertable",
                principalColumn: "UserId");
        }
    }
}
