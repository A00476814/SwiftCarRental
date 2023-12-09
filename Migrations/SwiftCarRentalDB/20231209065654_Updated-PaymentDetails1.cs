using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwiftCarRental.Migrations.SwiftCarRentalDB
{
    /// <inheritdoc />
    public partial class UpdatedPaymentDetails1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "PaymentDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProvinceState",
                table: "PaymentDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "PaymentDetails");

            migrationBuilder.DropColumn(
                name: "ProvinceState",
                table: "PaymentDetails");
        }
    }
}
