using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Web.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Inventories",
                keyColumn: "Id",
                keyValue: 1,
                column: "UpdatedAtUtc",
                value: new DateTime(2026, 2, 28, 10, 10, 10, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Inventories",
                keyColumn: "Id",
                keyValue: 2,
                column: "UpdatedAtUtc",
                value: new DateTime(2026, 2, 28, 10, 10, 10, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Inventories",
                keyColumn: "Id",
                keyValue: 3,
                column: "UpdatedAtUtc",
                value: new DateTime(2026, 2, 28, 10, 10, 10, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Inventories",
                keyColumn: "Id",
                keyValue: 4,
                column: "UpdatedAtUtc",
                value: new DateTime(2026, 2, 28, 10, 10, 10, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Inventories",
                keyColumn: "Id",
                keyValue: 5,
                column: "UpdatedAtUtc",
                value: new DateTime(2026, 2, 28, 10, 10, 10, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Inventories",
                keyColumn: "Id",
                keyValue: 6,
                column: "UpdatedAtUtc",
                value: new DateTime(2026, 2, 28, 10, 10, 10, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Inventories",
                keyColumn: "Id",
                keyValue: 7,
                column: "UpdatedAtUtc",
                value: new DateTime(2026, 2, 28, 10, 10, 10, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Inventories",
                keyColumn: "Id",
                keyValue: 8,
                column: "UpdatedAtUtc",
                value: new DateTime(2026, 2, 28, 10, 10, 10, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Inventories",
                keyColumn: "Id",
                keyValue: 1,
                column: "UpdatedAtUtc",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Inventories",
                keyColumn: "Id",
                keyValue: 2,
                column: "UpdatedAtUtc",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Inventories",
                keyColumn: "Id",
                keyValue: 3,
                column: "UpdatedAtUtc",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Inventories",
                keyColumn: "Id",
                keyValue: 4,
                column: "UpdatedAtUtc",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Inventories",
                keyColumn: "Id",
                keyValue: 5,
                column: "UpdatedAtUtc",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Inventories",
                keyColumn: "Id",
                keyValue: 6,
                column: "UpdatedAtUtc",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Inventories",
                keyColumn: "Id",
                keyValue: 7,
                column: "UpdatedAtUtc",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Inventories",
                keyColumn: "Id",
                keyValue: 8,
                column: "UpdatedAtUtc",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
