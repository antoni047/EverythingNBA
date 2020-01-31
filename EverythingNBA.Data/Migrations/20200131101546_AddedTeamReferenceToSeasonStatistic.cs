using Microsoft.EntityFrameworkCore.Migrations;

namespace EverythingNBA.Data.Migrations
{
    public partial class AddedTeamReferenceToSeasonStatistic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SingleSeasonStatistics_Teams_TeamId",
                table: "SingleSeasonStatistics");

            migrationBuilder.AlterColumn<int>(
                name: "TeamId",
                table: "SingleSeasonStatistics",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SingleSeasonStatistics_Teams_TeamId",
                table: "SingleSeasonStatistics",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SingleSeasonStatistics_Teams_TeamId",
                table: "SingleSeasonStatistics");

            migrationBuilder.AlterColumn<int>(
                name: "TeamId",
                table: "SingleSeasonStatistics",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_SingleSeasonStatistics_Teams_TeamId",
                table: "SingleSeasonStatistics",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
