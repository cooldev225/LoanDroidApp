using Microsoft.EntityFrameworkCore.Migrations;

namespace LoanDroidApp.Migrations
{
    public partial class accountpayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountPayment",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    GatewayUserName = table.Column<string>(nullable: true),
                    GatewayPassword = table.Column<string>(nullable: true),
                    GatewayEmail = table.Column<string>(nullable: true),
                    GatewayUrl = table.Column<string>(nullable: true),
                    GatewayName = table.Column<string>(nullable: true),
                    CardFirstName = table.Column<string>(nullable: true),
                    CardLastName = table.Column<string>(nullable: true),
                    CardNumber = table.Column<string>(nullable: true),
                    CardExpirationDate = table.Column<string>(nullable: true),
                    CardAddress1 = table.Column<string>(nullable: true),
                    CardAddress2 = table.Column<string>(nullable: true),
                    BankCountry = table.Column<string>(nullable: true),
                    BankRoutingNumber = table.Column<string>(nullable: true),
                    BankCurrency = table.Column<string>(nullable: true),
                    BankAccountHolder = table.Column<string>(nullable: true),
                    BankName = table.Column<string>(nullable: true),
                    BankStreet = table.Column<string>(nullable: true),
                    BankCity = table.Column<string>(nullable: true),
                    BankRegion = table.Column<string>(nullable: true),
                    BankSwiftBicNumber = table.Column<string>(nullable: true),
                    BankIBANNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountPayment", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountPayment_Id",
                table: "AccountPayment",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountPayment");
        }
    }
}
