using Microsoft.EntityFrameworkCore.Migrations;

namespace EverythingNBA.Data.Migrations
{
    public partial class RenamedWinnerPointsAndLoserPoints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoserPoints",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "WinnerPoints",
                table: "Games");

            migrationBuilder.AddColumn<int>(
                name: "Team2Points",
                table: "Games",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeamHostPoints",
                table: "Games",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Team2Points",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "TeamHostPoints",
                table: "Games");

            migrationBuilder.AddColumn<int>(
                name: "LoserPoints",
                table: "Games",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WinnerPoints",
                table: "Games",
                type: "int",
                nullable: true);
        }
    }
}
