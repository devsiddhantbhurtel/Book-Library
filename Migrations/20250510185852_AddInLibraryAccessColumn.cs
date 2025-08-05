using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookLibrarySystem.Migrations
{
    /// <inheritdoc />
    public partial class AddInLibraryAccessColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Books",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "InLibraryAccess",
                table: "Books",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "InLibraryAccess",
                table: "Books");
        }
    }
}
