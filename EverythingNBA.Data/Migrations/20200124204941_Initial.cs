using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EverythingNBA.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CloudinaryImages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PicturePublicId = table.Column<string>(nullable: true),
                    PictureUrl = table.Column<string>(nullable: true),
                    PictureThumbnailUrl = table.Column<string>(nullable: true),
                    Length = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudinaryImages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    CloudinaryImageId = table.Column<int>(nullable: true),
                    Conference = table.Column<int>(nullable: false),
                    Venue = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teams_CloudinaryImages_CloudinaryImageId",
                        column: x => x.CloudinaryImageId,
                        principalTable: "CloudinaryImages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    TeamId = table.Column<int>(nullable: true),
                    RookieYear = table.Column<int>(nullable: true),
                    Age = table.Column<int>(nullable: false),
                    Height = table.Column<int>(nullable: false),
                    Weight = table.Column<int>(nullable: false),
                    Position = table.Column<int>(nullable: false),
                    IsStarter = table.Column<bool>(nullable: false),
                    CloudinaryImageId = table.Column<int>(nullable: true),
                    ShirtNumber = table.Column<int>(nullable: false),
                    CurrentAvereagePoints = table.Column<double>(nullable: false),
                    CurrentAverageAssists = table.Column<double>(nullable: false),
                    CurrentAverageRebounds = table.Column<double>(nullable: false),
                    CurrentAverageBlocks = table.Column<double>(nullable: false),
                    CurrentAverageSteals = table.Column<double>(nullable: false),
                    CurrentFieldGoalPercentage = table.Column<double>(nullable: false),
                    CurrentThreePercentage = table.Column<double>(nullable: false),
                    CurrentFreeThrowPercentage = table.Column<double>(nullable: false),
                    CareerAvereagePoints = table.Column<double>(nullable: false),
                    CareerAverageAssists = table.Column<double>(nullable: false),
                    CareerAverageRebounds = table.Column<double>(nullable: false),
                    CareerAverageBlocks = table.Column<double>(nullable: false),
                    CareerAverageSteals = table.Column<double>(nullable: false),
                    CareerAverageFieldGoalPercentage = table.Column<double>(nullable: false),
                    CareerAverageThreePercentage = table.Column<double>(nullable: false),
                    CareerAverageFreeThrowPercentage = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Players_CloudinaryImages_CloudinaryImageId",
                        column: x => x.CloudinaryImageId,
                        principalTable: "CloudinaryImages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Players_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Seasons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(nullable: false),
                    TitleWinnerId = table.Column<int>(nullable: true),
                    PlayoffId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seasons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seasons_Teams_TitleWinnerId",
                        column: x => x.TitleWinnerId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Awards",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    WinnerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Awards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Awards_Players_WinnerId",
                        column: x => x.WinnerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AllStarTeams",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    SeasonId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllStarTeams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AllStarTeams_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeasonId = table.Column<int>(nullable: true),
                    TeamHostId = table.Column<int>(nullable: true),
                    Team2Id = table.Column<int>(nullable: true),
                    WinnerId = table.Column<int>(nullable: true),
                    WinnerPoints = table.Column<int>(nullable: true),
                    LoserPoints = table.Column<int>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Games_Teams_Team2Id",
                        column: x => x.Team2Id,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Games_Teams_TeamHostId",
                        column: x => x.TeamHostId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Games_Teams_WinnerId",
                        column: x => x.WinnerId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SingleSeasonStatistics",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeasonId = table.Column<int>(nullable: true),
                    Wins = table.Column<int>(nullable: false),
                    Losses = table.Column<int>(nullable: false),
                    TeamId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SingleSeasonStatistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SingleSeasonStatistics_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SingleSeasonStatistics_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AllStarTeamsPlayers",
                columns: table => new
                {
                    PlayerId = table.Column<int>(nullable: false),
                    AllStarTeamId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllStarTeamsPlayers", x => new { x.AllStarTeamId, x.PlayerId });
                    table.ForeignKey(
                        name: "FK_AllStarTeamsPlayers_AllStarTeams_AllStarTeamId",
                        column: x => x.AllStarTeamId,
                        principalTable: "AllStarTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AllStarTeamsPlayers_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Series",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Team1Id = table.Column<int>(nullable: true),
                    Team2Id = table.Column<int>(nullable: true),
                    WinnerId = table.Column<int>(nullable: true),
                    WinnerGamesWon = table.Column<int>(nullable: true),
                    LoserGamesWon = table.Column<int>(nullable: true),
                    Game1Id = table.Column<int>(nullable: true),
                    Game2Id = table.Column<int>(nullable: true),
                    Game3Id = table.Column<int>(nullable: true),
                    Game4Id = table.Column<int>(nullable: true),
                    Game5Id = table.Column<int>(nullable: true),
                    Game6Id = table.Column<int>(nullable: true),
                    Game7Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Series", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Series_Games_Game1Id",
                        column: x => x.Game1Id,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Series_Games_Game2Id",
                        column: x => x.Game2Id,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Series_Games_Game3Id",
                        column: x => x.Game3Id,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Series_Games_Game4Id",
                        column: x => x.Game4Id,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Series_Games_Game5Id",
                        column: x => x.Game5Id,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Series_Games_Game6Id",
                        column: x => x.Game6Id,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Series_Games_Game7Id",
                        column: x => x.Game7Id,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Series_Teams_Team1Id",
                        column: x => x.Team1Id,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Series_Teams_Team2Id",
                        column: x => x.Team2Id,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Series_Teams_WinnerId",
                        column: x => x.WinnerId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SingleGameStatistics",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameId = table.Column<int>(nullable: false),
                    PlayerId = table.Column<int>(nullable: false),
                    MinutesPlayed = table.Column<int>(nullable: false),
                    Points = table.Column<double>(nullable: false),
                    Assists = table.Column<double>(nullable: false),
                    Rebounds = table.Column<double>(nullable: false),
                    Steals = table.Column<double>(nullable: false),
                    Blocks = table.Column<double>(nullable: false),
                    FieldGoalsMade = table.Column<double>(nullable: false),
                    FieldgoalAttempts = table.Column<double>(nullable: false),
                    Fielgoalpercentage = table.Column<double>(nullable: false),
                    ThreeMade = table.Column<double>(nullable: false),
                    ThreeAttempts = table.Column<double>(nullable: false),
                    ThreePercentage = table.Column<double>(nullable: false),
                    FreeThrowsMade = table.Column<double>(nullable: false),
                    FreethrowAttempts = table.Column<double>(nullable: false),
                    FreeThrowPercentage = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SingleGameStatistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SingleGameStatistics_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SingleGameStatistics_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Playoffs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeasonId = table.Column<int>(nullable: true),
                    WesternQuarterFinalFirstId = table.Column<int>(nullable: true),
                    WesternQuarterFinalSecondId = table.Column<int>(nullable: true),
                    WesternQuarterFinalThirdId = table.Column<int>(nullable: true),
                    WesternQuarterFinalFourthId = table.Column<int>(nullable: true),
                    EasternQuarterFinalFirstId = table.Column<int>(nullable: true),
                    EasternQuarterFinalSecondId = table.Column<int>(nullable: true),
                    EasternQuarterFinalThirdId = table.Column<int>(nullable: true),
                    EasternQuarterFinalFourthId = table.Column<int>(nullable: true),
                    WesternSemiFinalFirstId = table.Column<int>(nullable: true),
                    WesternSemiFinalSecondId = table.Column<int>(nullable: true),
                    EasternSemiFinalFirstId = table.Column<int>(nullable: true),
                    EasternSemiFinalSecondId = table.Column<int>(nullable: true),
                    WesternFinalId = table.Column<int>(nullable: true),
                    EasternFinalId = table.Column<int>(nullable: true),
                    FinalId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Playoffs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Playoffs_Series_EasternFinalId",
                        column: x => x.EasternFinalId,
                        principalTable: "Series",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Playoffs_Series_EasternQuarterFinalFirstId",
                        column: x => x.EasternQuarterFinalFirstId,
                        principalTable: "Series",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Playoffs_Series_EasternQuarterFinalFourthId",
                        column: x => x.EasternQuarterFinalFourthId,
                        principalTable: "Series",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Playoffs_Series_EasternQuarterFinalSecondId",
                        column: x => x.EasternQuarterFinalSecondId,
                        principalTable: "Series",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Playoffs_Series_EasternQuarterFinalThirdId",
                        column: x => x.EasternQuarterFinalThirdId,
                        principalTable: "Series",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Playoffs_Series_EasternSemiFinalFirstId",
                        column: x => x.EasternSemiFinalFirstId,
                        principalTable: "Series",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Playoffs_Series_EasternSemiFinalSecondId",
                        column: x => x.EasternSemiFinalSecondId,
                        principalTable: "Series",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Playoffs_Series_FinalId",
                        column: x => x.FinalId,
                        principalTable: "Series",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Playoffs_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Playoffs_Series_WesternFinalId",
                        column: x => x.WesternFinalId,
                        principalTable: "Series",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Playoffs_Series_WesternQuarterFinalFirstId",
                        column: x => x.WesternQuarterFinalFirstId,
                        principalTable: "Series",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Playoffs_Series_WesternQuarterFinalFourthId",
                        column: x => x.WesternQuarterFinalFourthId,
                        principalTable: "Series",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Playoffs_Series_WesternQuarterFinalSecondId",
                        column: x => x.WesternQuarterFinalSecondId,
                        principalTable: "Series",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Playoffs_Series_WesternQuarterFinalThirdId",
                        column: x => x.WesternQuarterFinalThirdId,
                        principalTable: "Series",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Playoffs_Series_WesternSemiFinalFirstId",
                        column: x => x.WesternSemiFinalFirstId,
                        principalTable: "Series",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Playoffs_Series_WesternSemiFinalSecondId",
                        column: x => x.WesternSemiFinalSecondId,
                        principalTable: "Series",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AllStarTeams_SeasonId",
                table: "AllStarTeams",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_AllStarTeamsPlayers_PlayerId",
                table: "AllStarTeamsPlayers",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Awards_WinnerId",
                table: "Awards",
                column: "WinnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_SeasonId",
                table: "Games",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_Team2Id",
                table: "Games",
                column: "Team2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Games_TeamHostId",
                table: "Games",
                column: "TeamHostId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_WinnerId",
                table: "Games",
                column: "WinnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_CloudinaryImageId",
                table: "Players",
                column: "CloudinaryImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_TeamId",
                table: "Players",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Playoffs_EasternFinalId",
                table: "Playoffs",
                column: "EasternFinalId");

            migrationBuilder.CreateIndex(
                name: "IX_Playoffs_EasternQuarterFinalFirstId",
                table: "Playoffs",
                column: "EasternQuarterFinalFirstId");

            migrationBuilder.CreateIndex(
                name: "IX_Playoffs_EasternQuarterFinalFourthId",
                table: "Playoffs",
                column: "EasternQuarterFinalFourthId");

            migrationBuilder.CreateIndex(
                name: "IX_Playoffs_EasternQuarterFinalSecondId",
                table: "Playoffs",
                column: "EasternQuarterFinalSecondId");

            migrationBuilder.CreateIndex(
                name: "IX_Playoffs_EasternQuarterFinalThirdId",
                table: "Playoffs",
                column: "EasternQuarterFinalThirdId");

            migrationBuilder.CreateIndex(
                name: "IX_Playoffs_EasternSemiFinalFirstId",
                table: "Playoffs",
                column: "EasternSemiFinalFirstId");

            migrationBuilder.CreateIndex(
                name: "IX_Playoffs_EasternSemiFinalSecondId",
                table: "Playoffs",
                column: "EasternSemiFinalSecondId");

            migrationBuilder.CreateIndex(
                name: "IX_Playoffs_FinalId",
                table: "Playoffs",
                column: "FinalId");

            migrationBuilder.CreateIndex(
                name: "IX_Playoffs_SeasonId",
                table: "Playoffs",
                column: "SeasonId",
                unique: true,
                filter: "[SeasonId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Playoffs_WesternFinalId",
                table: "Playoffs",
                column: "WesternFinalId");

            migrationBuilder.CreateIndex(
                name: "IX_Playoffs_WesternQuarterFinalFirstId",
                table: "Playoffs",
                column: "WesternQuarterFinalFirstId");

            migrationBuilder.CreateIndex(
                name: "IX_Playoffs_WesternQuarterFinalFourthId",
                table: "Playoffs",
                column: "WesternQuarterFinalFourthId");

            migrationBuilder.CreateIndex(
                name: "IX_Playoffs_WesternQuarterFinalSecondId",
                table: "Playoffs",
                column: "WesternQuarterFinalSecondId");

            migrationBuilder.CreateIndex(
                name: "IX_Playoffs_WesternQuarterFinalThirdId",
                table: "Playoffs",
                column: "WesternQuarterFinalThirdId");

            migrationBuilder.CreateIndex(
                name: "IX_Playoffs_WesternSemiFinalFirstId",
                table: "Playoffs",
                column: "WesternSemiFinalFirstId");

            migrationBuilder.CreateIndex(
                name: "IX_Playoffs_WesternSemiFinalSecondId",
                table: "Playoffs",
                column: "WesternSemiFinalSecondId");

            migrationBuilder.CreateIndex(
                name: "IX_Seasons_TitleWinnerId",
                table: "Seasons",
                column: "TitleWinnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Series_Game1Id",
                table: "Series",
                column: "Game1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Series_Game2Id",
                table: "Series",
                column: "Game2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Series_Game3Id",
                table: "Series",
                column: "Game3Id");

            migrationBuilder.CreateIndex(
                name: "IX_Series_Game4Id",
                table: "Series",
                column: "Game4Id");

            migrationBuilder.CreateIndex(
                name: "IX_Series_Game5Id",
                table: "Series",
                column: "Game5Id");

            migrationBuilder.CreateIndex(
                name: "IX_Series_Game6Id",
                table: "Series",
                column: "Game6Id");

            migrationBuilder.CreateIndex(
                name: "IX_Series_Game7Id",
                table: "Series",
                column: "Game7Id");

            migrationBuilder.CreateIndex(
                name: "IX_Series_Team1Id",
                table: "Series",
                column: "Team1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Series_Team2Id",
                table: "Series",
                column: "Team2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Series_WinnerId",
                table: "Series",
                column: "WinnerId");

            migrationBuilder.CreateIndex(
                name: "IX_SingleGameStatistics_GameId",
                table: "SingleGameStatistics",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_SingleGameStatistics_PlayerId",
                table: "SingleGameStatistics",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_SingleSeasonStatistics_SeasonId",
                table: "SingleSeasonStatistics",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_SingleSeasonStatistics_TeamId",
                table: "SingleSeasonStatistics",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_CloudinaryImageId",
                table: "Teams",
                column: "CloudinaryImageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AllStarTeamsPlayers");

            migrationBuilder.DropTable(
                name: "Awards");

            migrationBuilder.DropTable(
                name: "Playoffs");

            migrationBuilder.DropTable(
                name: "SingleGameStatistics");

            migrationBuilder.DropTable(
                name: "SingleSeasonStatistics");

            migrationBuilder.DropTable(
                name: "AllStarTeams");

            migrationBuilder.DropTable(
                name: "Series");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Seasons");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "CloudinaryImages");
        }
    }
}
