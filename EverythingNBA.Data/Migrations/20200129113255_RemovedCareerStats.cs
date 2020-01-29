using Microsoft.EntityFrameworkCore.Migrations;

namespace EverythingNBA.Data.Migrations
{
    public partial class RemovedCareerStats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CareerAverageAssists",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "CareerAverageBlocks",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "CareerAverageFieldGoalPercentage",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "CareerAverageFreeThrowPercentage",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "CareerAverageRebounds",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "CareerAverageSteals",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "CareerAverageThreePercentage",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "CareerAvereagePoints",
                table: "Players");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CareerAverageAssists",
                table: "Players",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CareerAverageBlocks",
                table: "Players",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CareerAverageFieldGoalPercentage",
                table: "Players",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CareerAverageFreeThrowPercentage",
                table: "Players",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CareerAverageRebounds",
                table: "Players",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CareerAverageSteals",
                table: "Players",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CareerAverageThreePercentage",
                table: "Players",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CareerAvereagePoints",
                table: "Players",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
