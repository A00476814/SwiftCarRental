using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwiftCarRental.Migrations.SwiftCarRentalDB
{
    /// <inheritdoc />
    public partial class UpdatedPaymentDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameOnCard",
                table: "PaymentDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameOnCard",
                table: "PaymentDetails");
        }
    }
}
