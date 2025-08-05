using Microsoft.EntityFrameworkCore.Migrations;

namespace BookLibrarySystem.Data.Migrations
{
    public partial class AssignMembershipIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Update existing users with random membership IDs
            migrationBuilder.Sql(@"
                UPDATE Users 
                SET MembershipID = CONCAT(
                    FLOOR(RAND() * (99999999 - 10000000 + 1) + 10000000)
                )
                WHERE MembershipID IS NULL AND Role = 'User';
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE Users SET MembershipID = NULL WHERE Role = 'User';");
        }
    }
} 