using Microsoft.EntityFrameworkCore.Migrations;

namespace Catalog.Migrations
{
    public partial class GuildDetailsQuery : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GuildDetailsId",
                table: "Guilds",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GuildDetails",
                columns: table => new
                {
                    GuildDetailsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildDetails", x => x.GuildDetailsId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Guilds_GuildDetailsId",
                table: "Guilds",
                column: "GuildDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Guilds_GuildDetails_GuildDetailsId",
                table: "Guilds",
                column: "GuildDetailsId",
                principalTable: "GuildDetails",
                principalColumn: "GuildDetailsId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guilds_GuildDetails_GuildDetailsId",
                table: "Guilds");

            migrationBuilder.DropTable(
                name: "GuildDetails");

            migrationBuilder.DropIndex(
                name: "IX_Guilds_GuildDetailsId",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "GuildDetailsId",
                table: "Guilds");
        }
    }
}
