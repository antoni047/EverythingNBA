using Microsoft.EntityFrameworkCore.Migrations;

namespace EverythingNBA.Data.Migrations
{
    public partial class CreatedPlayerSeasonStatsEntityAndRemovedStatsFromPlayer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentAverageAssists",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "CurrentAverageBlocks",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "CurrentAverageRebounds",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "CurrentAverageSteals",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "CurrentAvereagePoints",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "CurrentFieldGoalPercentage",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "CurrentFreeThrowPercentage",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "CurrentThreePercentage",
                table: "Players");

            migrationBuilder.CreateTable(
                name: "PlayerSeasonStatistics",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerId = table.Column<int>(nullable: false),
                    SeasonId = table.Column<int>(nullable: false),
                    Points = table.Column<double>(nullable: false),
                    Assists = table.Column<double>(nullable: false),
                    AverageRebounds = table.Column<double>(nullable: false),
                    Blocks = table.Column<double>(nullable: false),
                    Steals = table.Column<double>(nullable: false),
                    FieldGoalPercentage = table.Column<double>(nullable: false),
                    ThreePercentage = table.Column<double>(nullable: false),
                    FreeThrowPercentage = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerSeasonStatistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerSeasonStatistics_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerSeasonStatistics_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerSeasonStatistics_PlayerId",
                table: "PlayerSeasonStatistics",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerSeasonStatistics_SeasonId",
                table: "PlayerSeasonStatistics",
                column: "SeasonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerSeasonStatistics");

            migrationBuilder.AddColumn<double>(
                name: "CurrentAverageAssists",
                table: "Players",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CurrentAverageBlocks",
                table: "Players",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CurrentAverageRebounds",
                table: "Players",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CurrentAverageSteals",
                table: "Players",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CurrentAvereagePoints",
                table: "Players",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CurrentFieldGoalPercentage",
                table: "Players",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CurrentFreeThrowPercentage",
                table: "Players",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CurrentThreePercentage",
                table: "Players",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
