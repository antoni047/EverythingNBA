using Microsoft.EntityFrameworkCore.Migrations;

namespace EverythingNBA.Data.Migrations
{
    public partial class AddedIsPlayoffGameColumnToGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPlayoffGame",
                table: "Games",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPlayoffGame",
                table: "Games");
        }
    }
}
