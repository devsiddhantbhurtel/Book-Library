using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookLibrarySystem.Migrations
{
    /// <inheritdoc />
    public partial class FixBookPublisherPrimaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BookPublishers",
                table: "BookPublishers");
            migrationBuilder.AddPrimaryKey(
                name: "PK_BookPublishers",
                table: "BookPublishers",
                columns: new[] { "BookID", "PublisherID" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BookPublishers",
                table: "BookPublishers");
            // If you had a previous PK, you could add it back here if needed
        }
    }
}
