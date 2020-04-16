using Microsoft.EntityFrameworkCore.Migrations;

namespace EverythingNBA.Data.Migrations
{
    public partial class AddedPrimaryAndSecondaryColorToTeamTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PrimaryColorHex",
                table: "Teams",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondaryColorHex",
                table: "Teams",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrimaryColorHex",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "SecondaryColorHex",
                table: "Teams");
        }
    }
}
