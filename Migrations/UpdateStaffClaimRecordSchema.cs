using Microsoft.EntityFrameworkCore.Migrations;

namespace BookLibrarySystem.Migrations
{
    public partial class UpdateStaffClaimRecordSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop existing table
            migrationBuilder.DropTable(
                name: "StaffClaimRecords");

            // Create new table with correct schema
            migrationBuilder.CreateTable(
                name: "StaffClaimRecords",
                columns: table => new
                {
                    StaffClaimRecordID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    ClaimTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffClaimRecords", x => x.StaffClaimRecordID);
                    table.ForeignKey(
                        name: "FK_StaffClaimRecords_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StaffClaimRecords_OrderID",
                table: "StaffClaimRecords",
                column: "OrderID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StaffClaimRecords");

            migrationBuilder.CreateTable(
                name: "StaffClaimRecords",
                columns: table => new
                {
                    StaffClaimRecordID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    ClaimTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffClaimRecords", x => x.StaffClaimRecordID);
                    table.ForeignKey(
                        name: "FK_StaffClaimRecords_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StaffClaimRecords_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StaffClaimRecords_OrderID",
                table: "StaffClaimRecords",
                column: "OrderID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StaffClaimRecords_UserID",
                table: "StaffClaimRecords",
                column: "UserID");
        }
    }
} 