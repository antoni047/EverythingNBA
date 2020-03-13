using Microsoft.EntityFrameworkCore.Migrations;

namespace EverythingNBA.Data.Migrations
{
    public partial class AddedSmallImageColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_CloudinaryImages_CloudinaryImageId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_CloudinaryImageId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "CloudinaryImageId",
                table: "Teams");

            migrationBuilder.AddColumn<int>(
                name: "FullImageId",
                table: "Teams",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SmallImageId",
                table: "Teams",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_FullImageId",
                table: "Teams",
                column: "FullImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_SmallImageId",
                table: "Teams",
                column: "SmallImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_CloudinaryImages_FullImageId",
                table: "Teams",
                column: "FullImageId",
                principalTable: "CloudinaryImages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_CloudinaryImages_SmallImageId",
                table: "Teams",
                column: "SmallImageId",
                principalTable: "CloudinaryImages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_CloudinaryImages_FullImageId",
                table: "Teams");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_CloudinaryImages_SmallImageId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_FullImageId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_SmallImageId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "FullImageId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "SmallImageId",
                table: "Teams");

            migrationBuilder.AddColumn<int>(
                name: "CloudinaryImageId",
                table: "Teams",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_CloudinaryImageId",
                table: "Teams",
                column: "CloudinaryImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_CloudinaryImages_CloudinaryImageId",
                table: "Teams",
                column: "CloudinaryImageId",
                principalTable: "CloudinaryImages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
