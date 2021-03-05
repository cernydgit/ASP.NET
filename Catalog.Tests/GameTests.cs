using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Catalog.Entities;
using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;

namespace Catalog.Tests
{
    public class GameTests
    {
        protected GameDbContext GameDbContext;
        protected WebApplicationFactory<Startup> AppFactory = new WebApplicationFactory<Startup>();

        [SetUp]
        public async Task SetUp()
        {
            GameDbContext = AppFactory.Services.CreateScope().ServiceProvider.GetService<GameDbContext>();
            await InitDatabase(3);
        }

        [Test]
        public async Task LoadNoTracking()
        {
            var players = await GameDbContext.Players.AsNoTracking().ToListAsync();
            Console.WriteLine(players.ToJson());
            Assert.AreNotEqual(0, players.Count);
        }

        [Test]
        public async Task LoadView()
        {
            var guilds = await GameDbContext.GuildsViews.AsNoTracking().ToListAsync();
            Console.WriteLine(guilds.ToJson());
            Assert.AreNotEqual(0, guilds.Count);
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
            var guilds = await GameDbContext.Guilds.Include(g => g.Players).ToListAsync();
            Console.WriteLine(guilds.ToJson());
            Assert.AreNotEqual(0, guilds[0].Players.Count);
        }

        [Test]
        public async Task LoadIncludeAsSplit()
        {
            var guilds = await GameDbContext.Guilds.Include(g => g.Players).AsSplitQuery().ToListAsync();
            Console.WriteLine(guilds.ToJson());
            Assert.AreNotEqual(0, guilds[0].Players.Count);
        }


        [Test]
        public async Task Update()
        {
            var g = await GameDbContext.Guilds.OrderBy(g => g.GuildId).FirstAsync();
            g.Name = "AAA";
            await GameDbContext.SaveChangesAsync();
        }

        private void CreateGuilds(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var g1 = new Guild { Name = "Guild_" + i };
                g1.Players.Add(new Player { Name = Guid.NewGuid().ToString() });
                g1.Players.Add(new Player { Name = Guid.NewGuid().ToString() });
                g1.Players.Add(new Player { Name = Guid.NewGuid().ToString() });
                GameDbContext.Guilds.Add(g1);
            }
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
            CreateGuilds(guildCount);
            await GameDbContext.SaveChangesAsync();
        }
    }
}