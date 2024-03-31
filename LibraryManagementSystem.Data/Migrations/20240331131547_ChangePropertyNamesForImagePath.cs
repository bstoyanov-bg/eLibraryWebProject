using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangePropertyNamesForImagePath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CoverImagePathUrl",
                table: "Books",
                newName: "ImageFilePath");

            migrationBuilder.RenameColumn(
                name: "ImagePathUrl",
                table: "Authors",
                newName: "ImageFilePath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageFilePath",
                table: "Books",
                newName: "CoverImagePathUrl");

            migrationBuilder.RenameColumn(
                name: "ImageFilePath",
                table: "Authors",
                newName: "ImagePathUrl");
        }
    }
}
