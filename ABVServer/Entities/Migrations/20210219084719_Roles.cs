using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class Roles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "061184c4-e5e3-42cd-a939-3db03ce10c6a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7e982de6-06fc-4a76-a939-a19d32549088");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3b893e6a-e11c-4136-9976-643d8f21d97c", "f3aa9441-4297-40f4-9d8a-18c08d79975f", "Viewer", "VIEWER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "26fe728a-b1dc-4f12-a673-e8b607594943", "3b66ef9e-b3ab-49e2-9ca8-014ad60990b5", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "26fe728a-b1dc-4f12-a673-e8b607594943");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3b893e6a-e11c-4136-9976-643d8f21d97c");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "061184c4-e5e3-42cd-a939-3db03ce10c6a", "c8bd3277-9d3f-412f-8b7c-07044f30d285", "Viewer", "VIEWER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7e982de6-06fc-4a76-a939-a19d32549088", "07ef2d60-9120-4da6-8ad8-3c1ab42cddab", "Administrator", "ADMINISTRATOR" });
        }
    }
}
