using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Catalog.Entities;
using Catalog.DTOs;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;

namespace Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuildsController : CrudControllerBase<GameDbContext>
    {
        public GuildsController(GameDbContext context, ILogger<GuildsController> logger) : base(context, logger) { }

        [HttpGet]
        public Task<ActionResult<IEnumerable<GuildSelectDto>>> GetGuilds() => Get<Guild, GuildSelectDto>();

        [HttpGet("{id}")]
        public Task<ActionResult<GuildSelectDto>> GetGuild(int id) => Get<Guild, GuildSelectDto>(id);

        [HttpPut("{id}")]
        //[ApiConventionMethod(typeof(DefaultApiConventions),nameof(DefaultApiConventions.Put))]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status304NotModified)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public Task<IActionResult> UpdateGuild(int id, GuildUpdateDto guildDTO) => Update<Guild, GuildUpdateDto>(id, guildDTO);

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public Task<ActionResult<GuildSelectDto>> InsertGuild(GuildInsertDto dto) => Insert<Guild, GuildInsertDto, GuildSelectDto>(dto, nameof(GetGuild));

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public Task<IActionResult> DeleteGuild(int id) => DeleteOne(Context.Guilds.Where(g => g.GuildId == id).Include(g => g.Players));
    }
}
