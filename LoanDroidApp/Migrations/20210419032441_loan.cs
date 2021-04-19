using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LoanDroidApp.Migrations
{
    public partial class loan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "AccountPayment",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "AccountPayment",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedDevice",
                table: "AccountPayment",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "AccountPayment",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "AccountPayment",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedDevice",
                table: "AccountPayment",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AccountPayment");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "AccountPayment");

            migrationBuilder.DropColumn(
                name: "CreatedDevice",
                table: "AccountPayment");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "AccountPayment");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "AccountPayment");

            migrationBuilder.DropColumn(
                name: "UpdatedDevice",
                table: "AccountPayment");
        }
    }
}
