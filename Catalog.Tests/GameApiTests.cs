using System.Threading.Tasks;
using NUnit.Framework;
using Catalog.Tests.Client;
using System.Linq;
using System.Net;
using System;

namespace Catalog.Tests
{
    public class GameApiTests : GameTestBase
    {
        protected GuildsClient Client { get; private set; }

        [SetUp]
        public override async Task SetUp()
        {
            await  base.SetUp();
            Client = new GuildsClient(AppFactory.CreateClient());

        }

        [Test]
        public async Task GetAll()
        {
            var guilds = await Client.GuildsGetAsync();
            CollectionAssert.IsNotEmpty(guilds);
        }

        [Test]
        public async Task GetPage()
        {
            var guilds = await Client.PageAsync(1, 1);
            Assert.AreEqual(1, guilds.Count);
            Assert.AreEqual(2, guilds.First().GuildId);
        }

        [Test]
        public async Task GetOne()
        {
            Assert.IsNotNull(await Client.GuildsGetAsync(1));
        }

        [Test]
        public async Task Insert()
        {
            var newGuild = await Client.GuildsPostAsync(new GuildInsertDto { Name = "NewGuild" });
            Assert.IsNotNull(await Client.GuildsGetAsync(newGuild.GuildId));
        }

        [Test]
        public async Task Update()
        {
            var guild = await Client.GuildsGetAsync(1);
            var updateDTO = new GuildUpdateDto { GuildId = 1, Name = "NewName", Timestamp = guild.Timestamp };
            await Client.GuildsPutAsync(1,updateDTO);
            guild = await Client.GuildsGetAsync(1);
            Assert.AreEqual(updateDTO.Name, guild.Name);
        }

        [Test]
        public async Task UpdateConcurrency()
        {
            var guild1 = await Client.GuildsGetAsync(1);
            var guild2 = await Client.GuildsGetAsync(1);
            var updateDTO1 = new GuildUpdateDto { GuildId = 1, Name = "NewName", Timestamp = guild1.Timestamp };
            var updateDTO2 = new GuildUpdateDto { GuildId = 1, Name = "SecondName", Timestamp = guild2.Timestamp };
            await Client.GuildsPutAsync(1, updateDTO1);
            var ex = Assert.ThrowsAsync<ApiException<ProblemDetails>>(async () => await Client.GuildsPutAsync(1, updateDTO2));
            Assert.AreEqual(HttpStatusCode.Conflict, (HttpStatusCode) ex.StatusCode);
        }

        [Test]
        public async Task Delete()
        {
            var guilds = await Client.GuildsGetAsync();
            await Client.GuildsDeleteAsync(guilds.First().GuildId);
            var ex = Assert.ThrowsAsync<ApiException<ProblemDetails>>(async () => await Client.GuildsGetAsync(guilds.First().GuildId));
            Assert.AreEqual(HttpStatusCode.NotFound, (HttpStatusCode)ex.StatusCode);
        }
    }
}