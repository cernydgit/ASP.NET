using Catalog.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSwag.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Controllers
{
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly GameDbContext gameDbContext;
        private readonly ILogger<GameController> logger;

        public GameController(GameDbContext gameDbContext, ILogger<GameController> logger)
        {
            this.gameDbContext = gameDbContext;
            this.logger = logger;
        }

        [HttpGet("Guilds")]
        [OpenApiIgnore]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<IEnumerable<Guild>>> GetGuilds()
        {

            using (logger.BeginScope("******* Getting guilds *********"))
            {
                logger.LogInformation("GetGuilds");
                var guild = new Guild();
                guild.Players.Add(new Player());
                gameDbContext.Guilds.Add(guild);

                await gameDbContext.SaveChangesAsync();
                return await gameDbContext.Guilds.Include(g => g.Players).ToListAsync();
            }
        }

        [HttpGet("Players")]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
        {
            var guild = await gameDbContext.Guilds.FirstAsync();
            guild.Players.Add(new Player());
            await gameDbContext.SaveChangesAsync();
            return await gameDbContext.Players.ToListAsync();
        }

    }
}
