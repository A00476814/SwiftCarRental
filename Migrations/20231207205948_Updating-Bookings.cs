using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwiftCarRental.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingBookings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_SwiftUser_SwiftUserId",
                table: "Booking");

            migrationBuilder.DropTable(
                name: "SwiftUser");

            migrationBuilder.DropIndex(
                name: "IX_Booking_SwiftUserId",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "SwiftUserId",
                table: "Booking");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SwiftUserId",
                table: "Booking",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SwiftUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LicenceNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SwiftUser", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Booking_SwiftUserId",
                table: "Booking",
                column: "SwiftUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_SwiftUser_SwiftUserId",
                table: "Booking",
                column: "SwiftUserId",
                principalTable: "SwiftUser",
                principalColumn: "Id");
        }
    }
}
