using Microsoft.EntityFrameworkCore.Migrations;

namespace EverythingNBA.Data.Migrations
{
    public partial class AddedWinnerTeamNameColumnToAwardsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WinnerTeamName",
                table: "Awards",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WinnerTeamName",
                table: "Awards");
        }
    }
}
