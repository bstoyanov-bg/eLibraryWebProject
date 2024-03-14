using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFilePathImagePathAndCreatedOnProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Ratings");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Ratings",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                comment: "Created On");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnDate",
                table: "LendedBooks",
                type: "datetime2",
                nullable: true,
                comment: "The date when the book was returned",
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true,
                oldComment: "The date when the book was returned");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LoanDate",
                table: "LendedBooks",
                type: "datetime2",
                nullable: false,
                comment: "The date when the book was borrowed",
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldComment: "The date when the book was borrowed");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Editions",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                comment: "Created On");

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "Editions",
                type: "nvarchar(260)",
                maxLength: 260,
                nullable: true,
                comment: "Path of the uploaded file");

            migrationBuilder.AlterColumn<string>(
                name: "CoverImagePathUrl",
                table: "Books",
                type: "nvarchar(260)",
                maxLength: 260,
                nullable: true,
                comment: "Cover image of the book",
                oldClrType: typeof(string),
                oldType: "nvarchar(2083)",
                oldMaxLength: 2083,
                oldNullable: true,
                oldComment: "Cover image of the book");

            migrationBuilder.AddColumn<string>(
                name: "ImagePathUrl",
                table: "Authors",
                type: "nvarchar(260)",
                maxLength: 260,
                nullable: true,
                comment: "Image of the author");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Editions");

            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "Editions");

            migrationBuilder.DropColumn(
                name: "ImagePathUrl",
                table: "Authors");

            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                table: "Ratings",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                comment: "Date of the rating");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "ReturnDate",
                table: "LendedBooks",
                type: "date",
                nullable: true,
                comment: "The date when the book was returned",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldComment: "The date when the book was returned");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "LoanDate",
                table: "LendedBooks",
                type: "date",
                nullable: false,
                comment: "The date when the book was borrowed",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldComment: "The date when the book was borrowed");

            migrationBuilder.AlterColumn<string>(
                name: "CoverImagePathUrl",
                table: "Books",
                type: "nvarchar(2083)",
                maxLength: 2083,
                nullable: true,
                comment: "Cover image of the book",
                oldClrType: typeof(string),
                oldType: "nvarchar(260)",
                oldMaxLength: 260,
                oldNullable: true,
                oldComment: "Cover image of the book");
        }
    }
}
