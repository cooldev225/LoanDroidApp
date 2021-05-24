using Microsoft.EntityFrameworkCore.Migrations;

namespace LoanDroidApp.Migrations
{
    public partial class investmentid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvestmenttId",
                table: "InvestmentStatus");

            migrationBuilder.AddColumn<long>(
                name: "InvestmentId",
                table: "InvestmentStatus",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvestmentId",
                table: "InvestmentStatus");

            migrationBuilder.AddColumn<long>(
                name: "InvestmenttId",
                table: "InvestmentStatus",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
