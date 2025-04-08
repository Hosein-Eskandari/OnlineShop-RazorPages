using Microsoft.EntityFrameworkCore.Migrations;

namespace DiscountManagement.Infrastructure.EFCore.Migrations
{
    public partial class changeColleagueDiscounttoDecimal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountRate",
                table: "CostumerDiscounts",
                type: "decimal(4,2)",
                precision: 4,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(17,3)",
                oldPrecision: 17,
                oldScale: 3);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountRate",
                table: "CostumerDiscounts",
                type: "decimal(17,3)",
                precision: 17,
                scale: 3,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(4,2)",
                oldPrecision: 4,
                oldScale: 2);

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
    }
}
