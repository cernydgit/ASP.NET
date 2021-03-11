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

    public class GameDbTests : GameTestBase
    {
        [Test]
        public async Task LoadNoTracking()
        {
            var players = await DbContext.Players.AsNoTracking().ToListAsync();
            Console.WriteLine(players.ToJson());
            Assert.AreNotEqual(0, players.Count);
        }

        [Test]
        public async Task ComputedColumn()
        {
            var guilds = await DbContext.Guilds.Select(g => new { Guild = g, PlayerCount = g.Players.Count }).ToListAsync();
            Console.WriteLine(guilds.ToJson());
            Assert.AreNotEqual(0, guilds[0].PlayerCount);
        }


        [Test]
        public async Task LoadExplicit()
        {
            var g = await DbContext.Guilds.OrderBy(g => g.GuildId).FirstAsync();
            await DbContext.Entry(g).Collection(g => g.Players).LoadAsync();
            Console.WriteLine(g.ToJson());
            Assert.AreNotEqual(0, g.Players.Count);
        }

        [Test]
        public async Task LoadInclude()
        {
            var guilds = await DbContext.Guilds.Include(g => g.Players).TagWith(nameof(LoadInclude)).ToListAsync();
            Console.WriteLine(guilds.ToJson());
            Assert.AreNotEqual(0, guilds[0].Players.Count);
        }

        [Test]
        public async Task LoadDerived()
        {
            var mp = await DbContext.MultiPlayers.ToListAsync();
            Console.WriteLine(mp.ToJson());
            Assert.AreNotEqual(0, mp.Count);
        }


        [Test]
        public async Task LoadIncludeAsSplit()
        {
            var guilds = await DbContext.Guilds.Include(g => g.Players).AsSplitQuery().ToListAsync();
            Console.WriteLine(guilds.ToJson());
            Assert.AreNotEqual(0, guilds[0].Players.Count);
        }

        [Test]
        public async Task LoadGuildDetailsByJoin()
        {
            var g = await DbContext.GetGuildDetailsByJoin().FirstAsync(g => g.GuildId == 1);
            Console.WriteLine(g.ToJson());
            Assert.AreNotEqual(0, g.PlayerCount);
        }

        [Test]
        public async Task LoadGuildDetailsByGroup()
        {
            var g = await DbContext.GetGuildDetailsByGroup().FirstAsync(g => g.GuildId == 1);
            Console.WriteLine(g.ToJson());
            Assert.AreNotEqual(0, g.PlayerCount);
        }

        [Test]
        public async Task Update()
        {
            var g = await DbContext.Guilds.OrderBy(g => g.GuildId).FirstAsync();
            g.Name = "AAA";
            g.Timestamp = new byte[5] { 1,2,3,4,5};
            await DbContext.SaveChangesAsync();
        }


        [Test]
        public void UpdateConcurrency()
        {
            Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () =>
            {

                var db1 = DbContext;
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
            var guilds = await DbContext.Guilds.ToListAsync();
            guilds.ForEach(g => DbContext.Guilds.Remove(g));
            await DbContext.SaveChangesAsync();
            var p = await DbContext.Players.FirstAsync();
            Assert.IsNull(p.GuildId);
        }
    }
}