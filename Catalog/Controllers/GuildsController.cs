using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Catalog.Entities;
using Catalog.DTOs;

using MapsterMapper;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuildsController : CrudControllerBase<GameDbContext>
    {
        public GuildsController(GameDbContext context, IMapper mapper, ILogger<GuildsController> logger) : base(context, mapper, logger) { }

        [HttpGet]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public Task<ActionResult<IEnumerable<GuildSelectDto>>> GetGuilds() => Get<Guild, GuildSelectDto>();

        [HttpGet("page")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public Task<ActionResult<IEnumerable<GuildSelectDto>>> GetGuildsPage(
            [FromQuery(Name = "skip")][Range(0, int.MaxValue)] int skip,
            [FromQuery(Name = "take")][Range(1, 200)] int take)
        {
            return GetPage<Guild, GuildSelectDto>(skip, take);
        }

        [HttpGet("{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public Task<ActionResult<GuildSelectDto>> GetGuild(int id) => Get<Guild, GuildSelectDto>(id);

        [HttpPut("{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),nameof(DefaultApiConventions.Put))]
        //[ProducesResponseType(StatusCodes.Status409Conflict)]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status304NotModified)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        public Task<IActionResult> UpdateGuild(int id, GuildUpdateDto guildDTO) => Update<Guild, GuildUpdateDto>(id, guildDTO);

        [HttpPost]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        public Task<ActionResult<GuildSelectDto>> InsertGuild(GuildInsertDto dto) => Insert<Guild, GuildInsertDto, GuildSelectDto>(dto, nameof(GetGuild));

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Delete))]
        public Task<IActionResult> DeleteGuild(int id) => DeleteOne(Context.Guilds.Where(g => g.GuildId == id).Include(g => g.Players));
    }
}
