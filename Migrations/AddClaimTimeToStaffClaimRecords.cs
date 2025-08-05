using Microsoft.EntityFrameworkCore.Migrations;

namespace BookLibrarySystem.Migrations
{
    public partial class AddClaimTimeToStaffClaimRecords : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ClaimTime",
                table: "StaffClaimRecords",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "StaffClaimRecords");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClaimTime",
                table: "StaffClaimRecords");

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "StaffClaimRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
} 