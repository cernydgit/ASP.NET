using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Catalog.Entities;
using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Catalog.Tests
{
    public class GameTests
    {
        protected GameDbContext GameDbContext;
        protected WebApplicationFactory<Startup> AppFactory = new WebApplicationFactory<Startup>();
        ILogger<GameTests> logger;

        [SetUp]
        public async Task SetUp()
        {
            GameDbContext = CreateDbContext();
            logger = AppFactory.Services.GetService<ILoggerFactory>().CreateLogger<GameTests>();
            await InitDatabase(3);
        }

        private GameDbContext CreateDbContext()
        {
            var db = AppFactory.Services.CreateScope().ServiceProvider.GetService<GameDbContext>();
            db.SaveChangesFailed += Db_SaveChangesFailed;
            return db;
        }

        private void Db_SaveChangesFailed(object sender, SaveChangesFailedEventArgs e)
        {
            logger.LogError(e.Exception.ToString());
        }

        [Test]
        public async Task Log()
        {
            logger.LogInformation("Simple message");
            logger.LogInformation("Parametrized message {Param1}", 123);

        }


        [Test]
        public async Task LoadNoTracking()
        {
            var players = await GameDbContext.Players.AsNoTracking().ToListAsync();
            Console.WriteLine(players.ToJson());
            Assert.AreNotEqual(0, players.Count);
        }

        [Test]
        public async Task ComputedColumn()
        {
            var guilds = await GameDbContext.Guilds.Select(g => new { Guild = g, PlayerCount = g.Players.Count }).ToListAsync();
            Console.WriteLine(guilds.ToJson());
            Assert.AreNotEqual(0, guilds[0].PlayerCount);
        }


        [Test]
        public async Task LoadExplicit()
        {
            var g = await GameDbContext.Guilds.OrderBy(g => g.GuildId).FirstAsync();
            await GameDbContext.Entry(g).Collection(g => g.Players).LoadAsync();
            Console.WriteLine(g.ToJson());
            Assert.AreNotEqual(0, g.Players.Count);
        }

        [Test]
        public async Task LoadInclude()
        {
            var guilds = await GameDbContext.Guilds.Include(g => g.Players).TagWith(nameof(LoadInclude)).ToListAsync();
            Console.WriteLine(guilds.ToJson());
            Assert.AreNotEqual(0, guilds[0].Players.Count);
        }

        [Test]
        public async Task LoadDerived()
        {
            var mp = await GameDbContext.MultiPlayers.ToListAsync();
            Console.WriteLine(mp.ToJson());
            Assert.AreNotEqual(0, mp.Count);
        }


        [Test]
        public async Task LoadIncludeAsSplit()
        {
            var guilds = await GameDbContext.Guilds.Include(g => g.Players).AsSplitQuery().ToListAsync();
            Console.WriteLine(guilds.ToJson());
            Assert.AreNotEqual(0, guilds[0].Players.Count);
        }

        //[Test]
        //public async Task LoadGuildDetailsBySet()
        //{
        //    var g = await GameDbContext.GuildDetails.FirstAsync(g => g.GuildId == 1);
        //    Console.WriteLine(g.ToJson());
        //    Assert.AreNotEqual(0, g.PlayerCount);
        //}

        //[Test]
        //[Ignore("Navigation to entity defined by query doesn't work")]
        //public async Task LoadGuildDetailsByInclude()
        //{
        //    //var d = await GameDbContext.GuildDetails.Include(g => g.Guild).ToListAsync();
        //    var d = await GameDbContext.GuildDetails.ToListAsync();
        //    var g = await GameDbContext.Guilds.Include(g => g.GuildDetails).FirstAsync();// g => g.GuildId == 1);
        //    Console.WriteLine(g.ToJson());
        //    Assert.AreNotEqual(0, g.GuildDetails.PlayerCount);
        //}


        //[Test]
        //public async Task LoadGuildDetailsByQyeryJoin()
        //{
        //    var g = await GameDbContext.Guilds.Join(GameDbContext.GuildDetails, g => g.GuildId, d => d.GuildId, (g, d) => new { Guild = g, GuildDetail = d }).FirstAsync();
        //    Console.WriteLine(g.ToJson());
        //    Assert.AreNotEqual(0, g.GuildDetail.PlayerCount);
        //}



        [Test]
        public async Task LoadGuildDetailsByJoin()
        {
            var g = await GameDbContext.GetGuildDetailsByJoin().FirstAsync(g => g.GuildId == 1);
            Console.WriteLine(g.ToJson());
            Assert.AreNotEqual(0, g.PlayerCount);
        }

        [Test]
        public async Task LoadGuildDetailsByGroup()
        {
            var g = await GameDbContext.GetGuildDetailsByGroup().FirstAsync(g => g.GuildId == 1);
            Console.WriteLine(g.ToJson());
            Assert.AreNotEqual(0, g.PlayerCount);
        }




        [Test]
        public async Task Update()
        {
            var g = await GameDbContext.Guilds.OrderBy(g => g.GuildId).FirstAsync();
            g.Name = "AAA";
            g.Timestamp = new byte[5] { 1,2,3,4,5};
            await GameDbContext.SaveChangesAsync();
        }


        [Test]
        public void UpdateConcurrency()
        {
            Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () =>
            {

                var db1 = GameDbContext;
                var db2 = CreateDbContext();
                var g1 = await db1.Guilds.OrderBy(g => g.GuildId).FirstAsync();
                var g2 = await db2.Guilds.OrderBy(g => g.GuildId).FirstAsync();
                g1.Name = "AAA";
                await db1.SaveChangesAsync();

                g2.Name = "BBB";
                await db2.SaveChangesAsync();
            });
        }


        [Test]
        public async Task Delete()
        {
            var guilds = await GameDbContext.Guilds.ToListAsync();
            guilds.ForEach(g => GameDbContext.Guilds.Remove(g));
            await GameDbContext.SaveChangesAsync();
            var p = await GameDbContext.Players.FirstAsync();
            Assert.IsNull(p.GuildId);
        }

        private void RecreateDatabase()
        {
            GameDbContext.Database.EnsureDeleted();
            //GameDbContext.Database.EnsureCreated();
            GameDbContext.Database.Migrate();
            Console.WriteLine("Applied migrations:\n " + string.Join(",\n", GameDbContext.Database.GetAppliedMigrations()));
        }

        private async Task InitDatabase(int guildCount = 3)
        {
            RecreateDatabase();


            // tags
            for (int i = 0; i < 3; i++)
            {
                GameDbContext.Tags.Add(new Tag());
            }
            await GameDbContext.SaveChangesAsync();

            // guilds
            for (int i = 0; i < guildCount; i++)
            {
                Player admin;
                var guild = new MultiGuild { Name = "Guild_" + i };

                // guild x players
                guild.Players.Add(admin = new Player());
                guild.Players.Add(new Player());
                guild.Players.Add(new MultiPlayer { MMR = 666});

                // guild x tags
                guild.Tags.AddRange(GameDbContext.Tags);

                GameDbContext.Guilds.Add(guild);
                await GameDbContext.SaveChangesAsync();
                guild.Admin = admin;
                await GameDbContext.SaveChangesAsync();
            }

            await GameDbContext.SaveChangesAsync();
        }
    }
}