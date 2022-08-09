using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NFTs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CHP = table.Column<int>(type: "int", nullable: false),
                    ATK = table.Column<int>(type: "int", nullable: false),
                    ATK_XP = table.Column<float>(type: "real", nullable: false),
                    DEF = table.Column<int>(type: "int", nullable: false),
                    DEF_XP = table.Column<float>(type: "real", nullable: false),
                    SPD = table.Column<int>(type: "int", nullable: false),
                    SPD_XP = table.Column<float>(type: "real", nullable: false),
                    ATK_SPD = table.Column<int>(type: "int", nullable: false),
                    ATK_SPD_XP = table.Column<float>(type: "real", nullable: false),
                    ATK_SPD_C = table.Column<int>(type: "int", nullable: false),
                    ATK_SPD_C_XP = table.Column<float>(type: "real", nullable: false),
                    DEF_SPD_D = table.Column<int>(type: "int", nullable: false),
                    DEF_SPD_D_XP = table.Column<float>(type: "real", nullable: false),
                    ATK_SPD_S = table.Column<int>(type: "int", nullable: false),
                    ATK_SPD_S_XP = table.Column<float>(type: "real", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Skill = table.Column<int>(type: "int", nullable: false),
                    SpriteSheetLink = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFTs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NFTs");
        }
    }
}
