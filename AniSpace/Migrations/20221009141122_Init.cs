using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AniSpace.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnimeBoxItemControls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnimeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnimeRating = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnimeAge = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnimeImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnimeTegs = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnimeOrigName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeBoxItemControls", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimeBoxItemControls");
        }
    }
}
