using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookLibrarySystem.Migrations
{
    /// <inheritdoc />
    public partial class AddDiscountEnhancements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookGenres_Books_BookID",
                table: "BookGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_BookGenres_Genres_GenreID",
                table: "BookGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_BookPublishers_Books_BookID",
                table: "BookPublishers");

            migrationBuilder.DropForeignKey(
                name: "FK_BookPublishers_Publishers_PublisherID",
                table: "BookPublishers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookPublishers",
                table: "BookPublishers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookGenres",
                table: "BookGenres");

            migrationBuilder.RenameTable(
                name: "BookPublishers",
                newName: "BookPublisher");

            migrationBuilder.RenameTable(
                name: "BookGenres",
                newName: "BookGenre");

            migrationBuilder.RenameColumn(
                name: "DiscountPercentage",
                table: "Discounts",
                newName: "DiscountValue");

            migrationBuilder.RenameIndex(
                name: "IX_BookPublishers_PublisherID",
                table: "BookPublisher",
                newName: "IX_BookPublisher_PublisherID");

            migrationBuilder.RenameIndex(
                name: "IX_BookGenres_GenreID",
                table: "BookGenre",
                newName: "IX_BookGenre_GenreID");

            migrationBuilder.AddColumn<int>(
                name: "DiscountType",
                table: "Discounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "StackingRule",
                table: "Discounts",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublicationDate",
                table: "Books",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<string>(
                name: "Language",
                table: "Books",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPublished",
                table: "Books",
                type: "tinyint(1)",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPhysical",
                table: "Books",
                type: "tinyint(1)",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "InLibraryAccess",
                table: "Books",
                type: "tinyint(1)",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Books",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Format",
                table: "Books",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Books",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookPublisher",
                table: "BookPublisher",
                columns: new[] { "BookID", "PublisherID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookGenre",
                table: "BookGenre",
                columns: new[] { "BookID", "GenreID" });

            migrationBuilder.AddForeignKey(
                name: "FK_BookGenre_Books_BookID",
                table: "BookGenre",
                column: "BookID",
                principalTable: "Books",
                principalColumn: "BookID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookGenre_Genres_GenreID",
                table: "BookGenre",
                column: "GenreID",
                principalTable: "Genres",
                principalColumn: "GenreID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookPublisher_Books_BookID",
                table: "BookPublisher",
                column: "BookID",
                principalTable: "Books",
                principalColumn: "BookID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookPublisher_Publishers_PublisherID",
                table: "BookPublisher",
                column: "PublisherID",
                principalTable: "Publishers",
                principalColumn: "PublisherID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookGenre_Books_BookID",
                table: "BookGenre");

            migrationBuilder.DropForeignKey(
                name: "FK_BookGenre_Genres_GenreID",
                table: "BookGenre");

            migrationBuilder.DropForeignKey(
                name: "FK_BookPublisher_Books_BookID",
                table: "BookPublisher");

            migrationBuilder.DropForeignKey(
                name: "FK_BookPublisher_Publishers_PublisherID",
                table: "BookPublisher");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookPublisher",
                table: "BookPublisher");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookGenre",
                table: "BookGenre");

            migrationBuilder.DropColumn(
                name: "DiscountType",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "StackingRule",
                table: "Discounts");

            migrationBuilder.RenameTable(
                name: "BookPublisher",
                newName: "BookPublishers");

            migrationBuilder.RenameTable(
                name: "BookGenre",
                newName: "BookGenres");

            migrationBuilder.RenameColumn(
                name: "DiscountValue",
                table: "Discounts",
                newName: "DiscountPercentage");

            migrationBuilder.RenameIndex(
                name: "IX_BookPublisher_PublisherID",
                table: "BookPublishers",
                newName: "IX_BookPublishers_PublisherID");

            migrationBuilder.RenameIndex(
                name: "IX_BookGenre_GenreID",
                table: "BookGenres",
                newName: "IX_BookGenres_GenreID");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublicationDate",
                table: "Books",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Language",
                keyValue: null,
                column: "Language",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Language",
                table: "Books",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPublished",
                table: "Books",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsPhysical",
                table: "Books",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "InLibraryAccess",
                table: "Books",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "ImageUrl",
                keyValue: null,
                column: "ImageUrl",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Books",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Format",
                keyValue: null,
                column: "Format",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Format",
                table: "Books",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Description",
                keyValue: null,
                column: "Description",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Books",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookPublishers",
                table: "BookPublishers",
                columns: new[] { "BookID", "PublisherID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookGenres",
                table: "BookGenres",
                columns: new[] { "BookID", "GenreID" });

            migrationBuilder.AddForeignKey(
                name: "FK_BookGenres_Books_BookID",
                table: "BookGenres",
                column: "BookID",
                principalTable: "Books",
                principalColumn: "BookID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookGenres_Genres_GenreID",
                table: "BookGenres",
                column: "GenreID",
                principalTable: "Genres",
                principalColumn: "GenreID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookPublishers_Books_BookID",
                table: "BookPublishers",
                column: "BookID",
                principalTable: "Books",
                principalColumn: "BookID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookPublishers_Publishers_PublisherID",
                table: "BookPublishers",
                column: "PublisherID",
                principalTable: "Publishers",
                principalColumn: "PublisherID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
