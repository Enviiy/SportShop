using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportShop.Migrations
{
    /// <inheritdoc />
    public partial class RemoveProductIdFromReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Products_ProductId",
                table: "Reviews");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Reviews",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "B1111111-1111-1111-1111-111111111111",
                column: "ConcurrencyStamp",
                value: "a4665ebf-4d00-4b9f-a410-ca0ac7bda141");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "C2222222-2222-2222-2222-222222222222",
                column: "ConcurrencyStamp",
                value: "c32e3396-b55d-49cf-adf7-4d86a60a27c6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "DA36513F-6393-43B8-A1E1-5CDF3DBEB6BF",
                column: "ConcurrencyStamp",
                value: "20ec68dd-6dd8-4b90-82e9-ef6448f5a09d");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "FA2325ED-7786-447A-BA83-93C89FE17D93",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "75fb0c91-9abd-4025-aeaf-3928566fccea", "AQAAAAIAAYagAAAAEOjoLfjDpvRaLBhvAdo0dKyIOh8hLDD7tivJec2pFgaNo0BWJky+EPaJPnYhEKfqCA==" });

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Products_ProductId",
                table: "Reviews",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Products_ProductId",
                table: "Reviews");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "B1111111-1111-1111-1111-111111111111",
                column: "ConcurrencyStamp",
                value: "2057f093-db48-4411-9e90-cd43da0885c0");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "C2222222-2222-2222-2222-222222222222",
                column: "ConcurrencyStamp",
                value: "5cc7d552-a1df-47a0-b1df-c32275f2b1f2");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "DA36513F-6393-43B8-A1E1-5CDF3DBEB6BF",
                column: "ConcurrencyStamp",
                value: "d5d76a2e-82ac-4cd7-9752-21f0e781b79e");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "FA2325ED-7786-447A-BA83-93C89FE17D93",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "7b8e3000-510c-413c-848e-e52b68390a00", "AQAAAAIAAYagAAAAEHlfdlutm5m0RUA5cKTp8puQKS6t+wEgPI1H2XG9c+aOQXPMtu/3+3OCa2rYYy2FPw==" });

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Products_ProductId",
                table: "Reviews",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
