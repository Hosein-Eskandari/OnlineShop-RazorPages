using Microsoft.EntityFrameworkCore.Migrations;

namespace DiscountManagement.Infrastructure.EFCore.Migrations
{
    public partial class changeColleagueDicounttofloat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "DiscountRate",
                table: "ColleagueDiscounts",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(17,3)",
                oldPrecision: 17,
                oldScale: 3);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountRate",
                table: "ColleagueDiscounts",
                type: "decimal(17,3)",
                precision: 17,
                scale: 3,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
