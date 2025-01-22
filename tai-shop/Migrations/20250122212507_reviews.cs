using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace tai_shop.Migrations
{
    /// <inheritdoc />
    public partial class reviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6a47f5de-7304-4bc8-a6c1-bebd3712e0ea");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6b4dd441-4af2-4870-bb72-755f8fe60b47");

            migrationBuilder.DropColumn(
                name: "Text",
                table: "Reviews");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "56a274f7-be57-4528-aed4-2f63a2c7469f", null, "User", "USER" },
                    { "d0ef7870-c8be-4850-9756-31632eb73000", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "56a274f7-be57-4528-aed4-2f63a2c7469f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d0ef7870-c8be-4850-9756-31632eb73000");

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6a47f5de-7304-4bc8-a6c1-bebd3712e0ea", null, "Admin", "ADMIN" },
                    { "6b4dd441-4af2-4870-bb72-755f8fe60b47", null, "User", "USER" }
                });
        }
    }
}
