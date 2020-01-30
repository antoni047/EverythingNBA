using Microsoft.EntityFrameworkCore.Migrations;

namespace EverythingNBA.Data.Migrations
{
    public partial class RemovedWinnercolumnFromSeries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Series_Teams_WinnerId",
                table: "Series");

            migrationBuilder.DropIndex(
                name: "IX_Series_WinnerId",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "WinnerId",
                table: "Series");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WinnerId",
                table: "Series",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Series_WinnerId",
                table: "Series",
                column: "WinnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Series_Teams_WinnerId",
                table: "Series",
                column: "WinnerId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
