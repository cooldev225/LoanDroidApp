using Microsoft.EntityFrameworkCore.Migrations;

namespace LoanDroidApp.Migrations
{
    public partial class userandinvestmentstatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Paid",
                table: "InvestmentStatus",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<long>(
                name: "TransactionId",
                table: "InvestmentStatus",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "activatedPaymentId",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Paid",
                table: "InvestmentStatus");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "InvestmentStatus");

            migrationBuilder.DropColumn(
                name: "activatedPaymentId",
                table: "AspNetUsers");
        }
    }
}
