using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prism.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedHashToRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RefreshToken",
                table: "Sessions",
                newName: "RefreshTokenHash");

            migrationBuilder.RenameIndex(
                name: "IX_Sessions_RefreshToken",
                table: "Sessions",
                newName: "IX_Sessions_RefreshTokenHash");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiresAt",
                table: "Clients",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpiresAt",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "RefreshTokenHash",
                table: "Sessions",
                newName: "RefreshToken");

            migrationBuilder.RenameIndex(
                name: "IX_Sessions_RefreshTokenHash",
                table: "Sessions",
                newName: "IX_Sessions_RefreshToken");
        }
    }
}
