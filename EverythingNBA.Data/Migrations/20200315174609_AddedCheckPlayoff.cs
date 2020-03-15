using Microsoft.EntityFrameworkCore.Migrations;

namespace EverythingNBA.Data.Migrations
{
    public partial class AddedCheckPlayoff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AreConferenceFinalsFinished",
                table: "Playoffs",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AreQuarterFinalsFinished",
                table: "Playoffs",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AreSemiFinalsFinished",
                table: "Playoffs",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AreConferenceFinalsFinished",
                table: "Playoffs");

            migrationBuilder.DropColumn(
                name: "AreQuarterFinalsFinished",
                table: "Playoffs");

            migrationBuilder.DropColumn(
                name: "AreSemiFinalsFinished",
                table: "Playoffs");
        }
    }
}
