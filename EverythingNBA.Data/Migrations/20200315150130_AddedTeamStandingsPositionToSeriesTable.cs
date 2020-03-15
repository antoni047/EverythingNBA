using Microsoft.EntityFrameworkCore.Migrations;

namespace EverythingNBA.Data.Migrations
{
    public partial class AddedTeamStandingsPositionToSeriesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Team1StandingsPosition",
                table: "Series",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Team2StandingsPosition",
                table: "Series",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Team1StandingsPosition",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "Team2StandingsPosition",
                table: "Series");
        }
    }
}
