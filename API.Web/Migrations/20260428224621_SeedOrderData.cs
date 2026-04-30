using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace API.Web.Migrations
{
    /// <inheritdoc />
    public partial class SeedOrderData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CreatedAtUtc", "CustomerId", "Status", "TotalAmount" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "1", 1349.98m },
                    { 2, new DateTime(2026, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "1", 179.98m },
                    { 3, new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "1", 699.99m },
                    { 4, new DateTime(2026, 4, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "1", 144.98m },
                    { 5, new DateTime(2026, 4, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, "2", 349.99m }
                });

            migrationBuilder.InsertData(
                table: "OrderItems",
                columns: new[] { "Id", "OrderId", "ProductId", "Quantity", "UnitPrice" },
                values: new object[,]
                {
                    { 1, 1, 1, 1, 1299.99m },
                    { 2, 1, 2, 1, 49.99m },
                    { 3, 2, 2, 1, 49.99m },
                    { 4, 2, 3, 1, 129.99m },
                    { 5, 3, 4, 1, 699.99m },
                    { 6, 4, 5, 1, 79.99m },
                    { 7, 4, 6, 1, 64.99m },
                    { 8, 5, 7, 1, 349.99m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
