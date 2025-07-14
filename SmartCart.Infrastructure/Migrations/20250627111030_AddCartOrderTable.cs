using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartCart.Infrastructure.SmartCart.Infrastructure.Migrations
{
    public partial class AddCartOrderTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create CartOrders table
            migrationBuilder.CreateTable(
                name: "CartOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CartId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartOrders", x => x.Id);
                });

            // Alter Product table to adjust precision for ProductWeight and OfferPercentage
            migrationBuilder.AlterColumn<decimal>(
                name: "ProductWeight",
                table: "Products",
                type: "decimal(10,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "OfferPercentage",
                table: "Products",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop CartOrders table
            migrationBuilder.DropTable(
                name: "CartOrders");

            // Revert ProductWeight and OfferPercentage to original types
            migrationBuilder.AlterColumn<decimal>(
                name: "ProductWeight",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,3)");

            migrationBuilder.AlterColumn<decimal>(
                name: "OfferPercentage",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");
        }
    }
}