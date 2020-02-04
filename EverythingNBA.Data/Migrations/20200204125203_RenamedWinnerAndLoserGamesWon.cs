using Microsoft.EntityFrameworkCore.Migrations;

namespace EverythingNBA.Data.Migrations
{
    public partial class RenamedWinnerAndLoserGamesWon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoserGamesWon",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "WinnerGamesWon",
                table: "Series");

            migrationBuilder.AddColumn<int>(
                name: "Team1GamesWon",
                table: "Series",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Team2GamesWon",
                table: "Series",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Team1GamesWon",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "Team2GamesWon",
                table: "Series");

            migrationBuilder.AddColumn<int>(
                name: "LoserGamesWon",
                table: "Series",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WinnerGamesWon",
                table: "Series",
                type: "int",
                nullable: true);
        }
    }
}
