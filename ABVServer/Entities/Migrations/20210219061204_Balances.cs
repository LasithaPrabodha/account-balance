using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class Balances : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c1344d4-d439-4a10-b153-5ce5086f2729");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dd135355-7a38-4d11-9b33-d10e395bcca4");

            migrationBuilder.AddColumn<double>(
                name: "Balance",
                table: "Accounts",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "061184c4-e5e3-42cd-a939-3db03ce10c6a", "c8bd3277-9d3f-412f-8b7c-07044f30d285", "Viewer", "VIEWER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7e982de6-06fc-4a76-a939-a19d32549088", "07ef2d60-9120-4da6-8ad8-3c1ab42cddab", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "061184c4-e5e3-42cd-a939-3db03ce10c6a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7e982de6-06fc-4a76-a939-a19d32549088");

            migrationBuilder.DropColumn(
                name: "Balance",
                table: "Accounts");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2c1344d4-d439-4a10-b153-5ce5086f2729", "4c5aaff2-e04f-483a-ac16-184599ae48ed", "Viewer", "VIEWER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "dd135355-7a38-4d11-9b33-d10e395bcca4", "5a011134-e0b9-405d-9fe1-e4e6ee1be115", "Administrator", "ADMINISTRATOR" });
        }
    }
}
