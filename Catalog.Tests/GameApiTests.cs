using System.Threading.Tasks;
using NUnit.Framework;
using Catalog.Tests.Client;

namespace Catalog.Tests
{
    public class GameApiTests : GameTestBase
    {
        [Test]
        public async Task Get()
        {
            var client = new GuildsClient(AppFactory.CreateClient());
            var guilds = await client.GetGetAsync();
        }
    }
}