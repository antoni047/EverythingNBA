using Microsoft.EntityFrameworkCore.Migrations;

namespace EverythingNBA.Data.Migrations
{
    public partial class AddSocialLinksRemovePercentages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fielgoalpercentage",
                table: "SingleGameStatistics");

            migrationBuilder.DropColumn(
                name: "FreeThrowPercentage",
                table: "SingleGameStatistics");

            migrationBuilder.DropColumn(
                name: "ThreePercentage",
                table: "SingleGameStatistics");

            migrationBuilder.RenameColumn(
                name: "FreethrowAttempts",
                table: "SingleGameStatistics",
                newName: "FreeThrowAttempts");

            migrationBuilder.RenameColumn(
                name: "FieldgoalAttempts",
                table: "SingleGameStatistics",
                newName: "FieldGoalAttempts");

            migrationBuilder.AddColumn<string>(
                name: "Instagram",
                table: "Teams",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Twitter",
                table: "Teams",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstagramLink",
                table: "Players",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwitterLink",
                table: "Players",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Instagram",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "Twitter",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "InstagramLink",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "TwitterLink",
                table: "Players");

            migrationBuilder.RenameColumn(
                name: "FreeThrowAttempts",
                table: "SingleGameStatistics",
                newName: "FreethrowAttempts");

            migrationBuilder.RenameColumn(
                name: "FieldGoalAttempts",
                table: "SingleGameStatistics",
                newName: "FieldgoalAttempts");

            migrationBuilder.AddColumn<double>(
                name: "Fielgoalpercentage",
                table: "SingleGameStatistics",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "FreeThrowPercentage",
                table: "SingleGameStatistics",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ThreePercentage",
                table: "SingleGameStatistics",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
