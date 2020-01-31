using Microsoft.EntityFrameworkCore.Migrations;

namespace EverythingNBA.Data.Migrations
{
    public partial class AddedSeasonForeignKeyToAwardsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SeasonId",
                table: "Awards",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Awards_SeasonId",
                table: "Awards",
                column: "SeasonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Awards_Seasons_SeasonId",
                table: "Awards",
                column: "SeasonId",
                principalTable: "Seasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Awards_Seasons_SeasonId",
                table: "Awards");

            migrationBuilder.DropIndex(
                name: "IX_Awards_SeasonId",
                table: "Awards");

            migrationBuilder.DropColumn(
                name: "SeasonId",
                table: "Awards");
        }
    }
}
