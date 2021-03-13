using System.Threading.Tasks;
using NUnit.Framework;
using Catalog.Tests.Client;
using System.Linq;
using System.Net;

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
        public async Task GetOne()
        {
            Assert.IsNotNull(await Client.GuildsGetAsync(1));
        }

        [Test]
        public async Task Insert()
        {
            var newGuild = await Client.GuildsPostAsync(new GuildInsertDTO { Name = "NewGuild" });
            Assert.IsNotNull(await Client.GuildsGetAsync(newGuild.GuildId));
        }

        [Test]
        public async Task Update()
        {
            var guild = await Client.GuildsGetAsync(1);
            var updateDTO = new GuildUpdateDTO { GuildId = 1, Name = "NewName", Timestamp = guild.Timestamp };
            await Client.GuildsPutAsync(1,updateDTO);
            guild = await Client.GuildsGetAsync(1);
            Assert.AreEqual(updateDTO.Name, guild.Name);
        }


        [Test]
        public async Task Delete()
        {
            var guilds = await Client.GuildsGetAsync();
            await Client.GuildsDeleteAsync(guilds.First().GuildId);
            var ex = Assert.ThrowsAsync<ApiException>(async () => await Client.GuildsGetAsync(guilds.First().GuildId));
            Assert.AreEqual(HttpStatusCode.NotFound, (HttpStatusCode)ex.StatusCode);
        }
    }
}