using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prism.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedDiscordProfiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "Clients",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DiscordProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    DiscordUserId = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    DiscordNickName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    DiscordGlobalName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    DiscordAvatarHash = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ClientId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordProfiles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiscordProfiles_ClientId",
                table: "DiscordProfiles",
                column: "ClientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiscordProfiles");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "Clients");
        }
    }
}
