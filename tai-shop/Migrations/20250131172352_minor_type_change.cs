using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace tai_shop.Migrations
{
    /// <inheritdoc />
    public partial class minor_type_change : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "25f6dbb4-e7a3-4bbc-a49a-57346c327bd9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c7cdbdf4-cc9a-4ea6-b44f-6c1b1750682e");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "ItemOrders",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2b2044d8-2ffe-45f7-9e7a-5e7c241e5811", null, "User", "USER" },
                    { "ad132aa0-afde-4e41-80d6-f23219b0ded9", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2b2044d8-2ffe-45f7-9e7a-5e7c241e5811");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ad132aa0-afde-4e41-80d6-f23219b0ded9");

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "ItemOrders",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "25f6dbb4-e7a3-4bbc-a49a-57346c327bd9", null, "User", "USER" },
                    { "c7cdbdf4-cc9a-4ea6-b44f-6c1b1750682e", null, "Admin", "ADMIN" }
                });
        }
    }
}
