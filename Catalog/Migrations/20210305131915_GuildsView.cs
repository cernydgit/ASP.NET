using Microsoft.EntityFrameworkCore.Migrations;

namespace Catalog.Migrations
{
    public partial class GuildsView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR ALTER VIEW [dbo].[GuildsView] AS
                SELECT          g.GuildId, g.Name, g.Created, COUNT(p.PlayerId) AS PlayerCount
                FROM            [dbo].[Guilds] AS g 
                LEFT OUTER JOIN [dbo].[Players] AS p ON g.GuildId = p.GuildId
                GROUP BY        g.GuildId, g.Name, g.Created");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW [dbo].[GuildsView]");
        }
    }
}
