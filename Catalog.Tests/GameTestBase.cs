using System.Threading.Tasks;

using Catalog.Entities;


namespace Catalog.Tests
{
    public class GameTestBase : IntegrationTest<GameDbContext> 
    {
        int guildCount = 3;

        protected override async Task SeedDatabase()
        {
            // tags
            for (int i = 0; i < 3; i++)
            {
                DbContext.Tags.Add(new Tag());
            }
            await DbContext.SaveChangesAsync();

            // guilds
            for (int i = 0; i < guildCount; i++)
            {
                Entities.Player admin;
                var guild = new MultiGuild { Name = "Guild_" + i };

                // guild x players
                guild.Players.Add(admin = new Entities.Player());
                guild.Players.Add(new Entities.Player());
                guild.Players.Add(new MultiPlayer { MMR = 666 });

                // guild x tags
                guild.Tags.AddRange(DbContext.Tags);

                DbContext.Guilds.Add(guild);
                await DbContext.SaveChangesAsync();
                guild.Admin = admin;
                await DbContext.SaveChangesAsync();
            }

            await DbContext.SaveChangesAsync();
        }
    }
}