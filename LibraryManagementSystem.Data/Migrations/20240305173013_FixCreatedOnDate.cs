using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LibraryManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixCreatedOnDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("8861d25f-dbcd-4f24-8f34-57ea3d6f6ba0"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("b566f840-e9c1-42da-b169-b1a38f292a61"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "Books",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                comment: "Created On",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 2, 16, 20, 17, 42, 664, DateTimeKind.Utc).AddTicks(1731),
                oldComment: "Created On");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "Authors",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                comment: "Created On",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 2, 16, 20, 17, 42, 663, DateTimeKind.Utc).AddTicks(8479),
                oldComment: "Created On");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("88caffe9-5075-4851-a103-f83a0aca00da"), "b23c0ec9-46bd-477c-8d6a-725735e94d97", "User", "USER" },
                    { new Guid("cebdf79f-cf72-4d2c-811e-2f5d509bf019"), "c08e232a-6e53-462c-8992-33ae3449df9c", "Administrator", "ADMINISTRATOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("88caffe9-5075-4851-a103-f83a0aca00da"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("cebdf79f-cf72-4d2c-811e-2f5d509bf019"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "Books",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 2, 16, 20, 17, 42, 664, DateTimeKind.Utc).AddTicks(1731),
                comment: "Created On",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()",
                oldComment: "Created On");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "Authors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 2, 16, 20, 17, 42, 663, DateTimeKind.Utc).AddTicks(8479),
                comment: "Created On",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()",
                oldComment: "Created On");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("8861d25f-dbcd-4f24-8f34-57ea3d6f6ba0"), "2804d939-1eba-4047-9a18-04b2326fc63d", "User", "USER" },
                    { new Guid("b566f840-e9c1-42da-b169-b1a38f292a61"), "9253f9c5-6c66-407a-a583-60cf7284f4b7", "Administrator", "ADMINISTRATOR" }
                });
        }
    }
}
