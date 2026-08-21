using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prism.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedDataInAppProfiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Profile",
                table: "AppProfiles",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "Data",
                table: "AppProfiles",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data",
                table: "AppProfiles");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "AppProfiles",
                newName: "Profile");
        }
    }
}
