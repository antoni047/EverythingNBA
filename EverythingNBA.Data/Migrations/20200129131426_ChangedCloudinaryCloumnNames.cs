using Microsoft.EntityFrameworkCore.Migrations;

namespace EverythingNBA.Data.Migrations
{
    public partial class ChangedCloudinaryCloumnNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PicturePublicId",
                table: "CloudinaryImages");

            migrationBuilder.DropColumn(
                name: "PictureThumbnailUrl",
                table: "CloudinaryImages");

            migrationBuilder.DropColumn(
                name: "PictureUrl",
                table: "CloudinaryImages");

            migrationBuilder.AddColumn<string>(
                name: "ImagePublicId",
                table: "CloudinaryImages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageThumbnailURL",
                table: "CloudinaryImages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageURL",
                table: "CloudinaryImages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePublicId",
                table: "CloudinaryImages");

            migrationBuilder.DropColumn(
                name: "ImageThumbnailURL",
                table: "CloudinaryImages");

            migrationBuilder.DropColumn(
                name: "ImageURL",
                table: "CloudinaryImages");

            migrationBuilder.AddColumn<string>(
                name: "PicturePublicId",
                table: "CloudinaryImages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PictureThumbnailUrl",
                table: "CloudinaryImages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PictureUrl",
                table: "CloudinaryImages",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
