using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LibraryManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreatedOn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("42d3d645-8a63-4571-9852-a32464de0d2f"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("c26e95d7-4387-4646-8e05-bb6566d69575"));

            migrationBuilder.AlterColumn<string>(
                name: "ISBN",
                table: "Books",
                type: "nvarchar(22)",
                maxLength: 22,
                nullable: true,
                comment: "International Standard Book Number",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "International Standard Book Number");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Books",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 2, 16, 20, 17, 42, 664, DateTimeKind.Utc).AddTicks(1731),
                comment: "Created On");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Authors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 2, 16, 20, 17, 42, 663, DateTimeKind.Utc).AddTicks(8479),
                comment: "Created On");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("8861d25f-dbcd-4f24-8f34-57ea3d6f6ba0"), "2804d939-1eba-4047-9a18-04b2326fc63d", "User", "USER" },
                    { new Guid("b566f840-e9c1-42da-b169-b1a38f292a61"), "9253f9c5-6c66-407a-a583-60cf7284f4b7", "Administrator", "ADMINISTRATOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("8861d25f-dbcd-4f24-8f34-57ea3d6f6ba0"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("b566f840-e9c1-42da-b169-b1a38f292a61"));

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Authors");

            migrationBuilder.AlterColumn<string>(
                name: "ISBN",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true,
                comment: "International Standard Book Number",
                oldClrType: typeof(string),
                oldType: "nvarchar(22)",
                oldMaxLength: 22,
                oldNullable: true,
                oldComment: "International Standard Book Number");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("42d3d645-8a63-4571-9852-a32464de0d2f"), null, "Administrator", "ADMINISTRATOR" },
                    { new Guid("c26e95d7-4387-4646-8e05-bb6566d69575"), null, "User", "USER" }
                });
        }
    }
}
