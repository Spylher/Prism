using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prism.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedIpAddressToSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "Sessions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastIpAddress",
                table: "Sessions",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "LastIpAddress",
                table: "Sessions");
        }
    }
}
