using Microsoft.EntityFrameworkCore.Migrations;

namespace spellboundapi.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Spells",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false),
                    School = table.Column<string>(nullable: false),
                    Level = table.Column<int>(nullable: false),
                    CastingTime = table.Column<string>(nullable: false),
                    Range = table.Column<string>(nullable: false),
                    Components = table.Column<string>(nullable: false),
                    Duration = table.Column<string>(nullable: false),
                    Materials = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    AtHigherLevels = table.Column<string>(nullable: false),
                    Reference = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spells", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Spells");
        }
    }
}
