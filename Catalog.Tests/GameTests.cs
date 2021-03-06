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

            for (int i = 0; i < guildCount; i++)
            {
                Player admin;
                var guild = new Guild { Name = "Guild_" + i };
                guild.Players.Add(admin = new Player { Name = Guid.NewGuid().ToString() });
                guild.Players.Add(new Player { Name = Guid.NewGuid().ToString() });
                guild.Players.Add(new Player { Name = Guid.NewGuid().ToString() });
                GameDbContext.Guilds.Add(guild);
                await GameDbContext.SaveChangesAsync();
                guild.Admin = admin;
                await GameDbContext.SaveChangesAsync();
            }

            await GameDbContext.SaveChangesAsync();
        }
    }
}