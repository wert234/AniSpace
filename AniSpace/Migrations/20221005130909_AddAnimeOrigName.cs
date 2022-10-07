using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AniSpace.Migrations
{
    public partial class AddAnimeOrigName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AnimeOrigName",
                table: "AnimeBoxItemControls",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnimeOrigName",
                table: "AnimeBoxItemControls");
        }
    }
}
