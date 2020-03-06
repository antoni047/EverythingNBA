using Microsoft.EntityFrameworkCore.Migrations;

namespace EverythingNBA.Data.Migrations
{
    public partial class RewrotePlayoffAndSeriesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Playoffs_Series_EasternFinalId",
                table: "Playoffs");

            migrationBuilder.DropForeignKey(
                name: "FK_Playoffs_Series_EasternQuarterFinalFirstId",
                table: "Playoffs");

            migrationBuilder.DropForeignKey(
                name: "FK_Playoffs_Series_EasternQuarterFinalFourthId",
                table: "Playoffs");

            migrationBuilder.DropForeignKey(
                name: "FK_Playoffs_Series_EasternQuarterFinalSecondId",
                table: "Playoffs");

            migrationBuilder.DropForeignKey(
                name: "FK_Playoffs_Series_EasternQuarterFinalThirdId",
                table: "Playoffs");

            migrationBuilder.DropForeignKey(
                name: "FK_Playoffs_Series_EasternSemiFinalFirstId",
                table: "Playoffs");

            migrationBuilder.DropForeignKey(
                name: "FK_Playoffs_Series_EasternSemiFinalSecondId",
                table: "Playoffs");

            migrationBuilder.DropForeignKey(
                name: "FK_Playoffs_Series_FinalId",
                table: "Playoffs");

            migrationBuilder.DropForeignKey(
                name: "FK_Playoffs_Series_WesternFinalId",
                table: "Playoffs");

            migrationBuilder.DropForeignKey(
                name: "FK_Playoffs_Series_WesternQuarterFinalFirstId",
                table: "Playoffs");

            migrationBuilder.DropForeignKey(
                name: "FK_Playoffs_Series_WesternQuarterFinalFourthId",
                table: "Playoffs");

            migrationBuilder.DropForeignKey(
                name: "FK_Playoffs_Series_WesternQuarterFinalSecondId",
                table: "Playoffs");

            migrationBuilder.DropForeignKey(
                name: "FK_Playoffs_Series_WesternQuarterFinalThirdId",
                table: "Playoffs");

            migrationBuilder.DropForeignKey(
                name: "FK_Playoffs_Series_WesternSemiFinalFirstId",
                table: "Playoffs");

            migrationBuilder.DropForeignKey(
                name: "FK_Playoffs_Series_WesternSemiFinalSecondId",
                table: "Playoffs");

            migrationBuilder.DropIndex(
                name: "IX_Playoffs_EasternFinalId",
                table: "Playoffs");

            migrationBuilder.DropIndex(
                name: "IX_Playoffs_EasternQuarterFinalFirstId",
                table: "Playoffs");

            migrationBuilder.DropIndex(
                name: "IX_Playoffs_EasternQuarterFinalFourthId",
                table: "Playoffs");

            migrationBuilder.DropIndex(
                name: "IX_Playoffs_EasternQuarterFinalSecondId",
                table: "Playoffs");

            migrationBuilder.DropIndex(
                name: "IX_Playoffs_EasternQuarterFinalThirdId",
                table: "Playoffs");

            migrationBuilder.DropIndex(
                name: "IX_Playoffs_EasternSemiFinalFirstId",
                table: "Playoffs");

            migrationBuilder.DropIndex(
                name: "IX_Playoffs_EasternSemiFinalSecondId",
                table: "Playoffs");

            migrationBuilder.DropIndex(
                name: "IX_Playoffs_FinalId",
                table: "Playoffs");

            migrationBuilder.DropIndex(
                name: "IX_Playoffs_WesternFinalId",
                table: "Playoffs");

            migrationBuilder.DropIndex(
                name: "IX_Playoffs_WesternQuarterFinalFirstId",
                table: "Playoffs");

            migrationBuilder.DropIndex(
                name: "IX_Playoffs_WesternQuarterFinalFourthId",
                table: "Playoffs");

            migrationBuilder.DropIndex(
                name: "IX_Playoffs_WesternQuarterFinalSecondId",
                table: "Playoffs");

            migrationBuilder.DropIndex(
                name: "IX_Playoffs_WesternQuarterFinalThirdId",
                table: "Playoffs");

            migrationBuilder.DropIndex(
                name: "IX_Playoffs_WesternSemiFinalFirstId",
                table: "Playoffs");

            migrationBuilder.DropIndex(
                name: "IX_Playoffs_WesternSemiFinalSecondId",
                table: "Playoffs");

            migrationBuilder.DropColumn(
                name: "EasternFinalId",
                table: "Playoffs");

            migrationBuilder.DropColumn(
                name: "EasternQuarterFinalFirstId",
                table: "Playoffs");

            migrationBuilder.DropColumn(
                name: "EasternQuarterFinalFourthId",
                table: "Playoffs");

            migrationBuilder.DropColumn(
                name: "EasternQuarterFinalSecondId",
                table: "Playoffs");

            migrationBuilder.DropColumn(
                name: "EasternQuarterFinalThirdId",
                table: "Playoffs");

            migrationBuilder.DropColumn(
                name: "EasternSemiFinalFirstId",
                table: "Playoffs");

            migrationBuilder.DropColumn(
                name: "EasternSemiFinalSecondId",
                table: "Playoffs");

            migrationBuilder.DropColumn(
                name: "FinalId",
                table: "Playoffs");

            migrationBuilder.DropColumn(
                name: "WesternFinalId",
                table: "Playoffs");

            migrationBuilder.DropColumn(
                name: "WesternQuarterFinalFirstId",
                table: "Playoffs");

            migrationBuilder.DropColumn(
                name: "WesternQuarterFinalFourthId",
                table: "Playoffs");

            migrationBuilder.DropColumn(
                name: "WesternQuarterFinalSecondId",
                table: "Playoffs");

            migrationBuilder.DropColumn(
                name: "WesternQuarterFinalThirdId",
                table: "Playoffs");

            migrationBuilder.DropColumn(
                name: "WesternSemiFinalFirstId",
                table: "Playoffs");

            migrationBuilder.DropColumn(
                name: "WesternSemiFinalSecondId",
                table: "Playoffs");

            migrationBuilder.AddColumn<string>(
                name: "Conference",
                table: "Series",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PlayoffId",
                table: "Series",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Stage",
                table: "Series",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "StageNumber",
                table: "Series",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Series_PlayoffId",
                table: "Series",
                column: "PlayoffId");

            migrationBuilder.AddForeignKey(
                name: "FK_Series_Playoffs_PlayoffId",
                table: "Series",
                column: "PlayoffId",
                principalTable: "Playoffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Series_Playoffs_PlayoffId",
                table: "Series");

            migrationBuilder.DropIndex(
                name: "IX_Series_PlayoffId",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "Conference",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "PlayoffId",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "Stage",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "StageNumber",
                table: "Series");

            migrationBuilder.AddColumn<int>(
                name: "EasternFinalId",
                table: "Playoffs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EasternQuarterFinalFirstId",
                table: "Playoffs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EasternQuarterFinalFourthId",
                table: "Playoffs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EasternQuarterFinalSecondId",
                table: "Playoffs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EasternQuarterFinalThirdId",
                table: "Playoffs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EasternSemiFinalFirstId",
                table: "Playoffs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EasternSemiFinalSecondId",
                table: "Playoffs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FinalId",
                table: "Playoffs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WesternFinalId",
                table: "Playoffs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WesternQuarterFinalFirstId",
                table: "Playoffs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WesternQuarterFinalFourthId",
                table: "Playoffs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WesternQuarterFinalSecondId",
                table: "Playoffs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WesternQuarterFinalThirdId",
                table: "Playoffs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WesternSemiFinalFirstId",
                table: "Playoffs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WesternSemiFinalSecondId",
                table: "Playoffs",
                type: "int",
                nullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Playoffs_Series_EasternFinalId",
                table: "Playoffs",
                column: "EasternFinalId",
                principalTable: "Series",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Playoffs_Series_EasternQuarterFinalFirstId",
                table: "Playoffs",
                column: "EasternQuarterFinalFirstId",
                principalTable: "Series",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Playoffs_Series_EasternQuarterFinalFourthId",
                table: "Playoffs",
                column: "EasternQuarterFinalFourthId",
                principalTable: "Series",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Playoffs_Series_EasternQuarterFinalSecondId",
                table: "Playoffs",
                column: "EasternQuarterFinalSecondId",
                principalTable: "Series",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Playoffs_Series_EasternQuarterFinalThirdId",
                table: "Playoffs",
                column: "EasternQuarterFinalThirdId",
                principalTable: "Series",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Playoffs_Series_EasternSemiFinalFirstId",
                table: "Playoffs",
                column: "EasternSemiFinalFirstId",
                principalTable: "Series",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Playoffs_Series_EasternSemiFinalSecondId",
                table: "Playoffs",
                column: "EasternSemiFinalSecondId",
                principalTable: "Series",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Playoffs_Series_FinalId",
                table: "Playoffs",
                column: "FinalId",
                principalTable: "Series",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Playoffs_Series_WesternFinalId",
                table: "Playoffs",
                column: "WesternFinalId",
                principalTable: "Series",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Playoffs_Series_WesternQuarterFinalFirstId",
                table: "Playoffs",
                column: "WesternQuarterFinalFirstId",
                principalTable: "Series",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Playoffs_Series_WesternQuarterFinalFourthId",
                table: "Playoffs",
                column: "WesternQuarterFinalFourthId",
                principalTable: "Series",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Playoffs_Series_WesternQuarterFinalSecondId",
                table: "Playoffs",
                column: "WesternQuarterFinalSecondId",
                principalTable: "Series",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Playoffs_Series_WesternQuarterFinalThirdId",
                table: "Playoffs",
                column: "WesternQuarterFinalThirdId",
                principalTable: "Series",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Playoffs_Series_WesternSemiFinalFirstId",
                table: "Playoffs",
                column: "WesternSemiFinalFirstId",
                principalTable: "Series",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Playoffs_Series_WesternSemiFinalSecondId",
                table: "Playoffs",
                column: "WesternSemiFinalSecondId",
                principalTable: "Series",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
