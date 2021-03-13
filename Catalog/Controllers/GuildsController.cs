using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Catalog.Entities;
using Mapster;

namespace Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuildsController : ControllerBase
    {
        private readonly GameDbContext _context;

        public GuildsController(GameDbContext context)
        {
            _context = context;
        }

        // GET: api/Guilds
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GuildSelectDTO>>> GetGuilds()
        {
            return await _context.Guilds.ProjectToType<GuildSelectDTO>().ToListAsync();
        }

        // GET: api/Guilds/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GuildSelectDTO>> GetGuild(int id)
        {
            var guild = await _context.Guilds.FindAsync(id);

            if (guild == null)
            {
                return NotFound();
            }

            return guild.Adapt<GuildSelectDTO>();
        }

        // PUT: api/Guilds/5
        [HttpPut("{id}")]
        //[ApiConventionMethod(typeof(DefaultApiConventions),nameof(DefaultApiConventions.Put))]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status304NotModified)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateGuild(int id, GuildUpdateDTO guildDTO)
        {
            if (id != guildDTO.GuildId)
            {
                return BadRequest();
            }

            var guild = await _context.Guilds.FirstOrDefaultAsync(g => g.GuildId == guildDTO.GuildId);

            if (guild == null)
            {
                return NotFound();
            }

            //not working for default values
            //var guild = new Guild { GuildId = guildDTO.GuildId };
            //_context.Attach(guild);
            guildDTO.Adapt(guild);
            _context.Entry(guild).Property(nameof(guild.Timestamp)).OriginalValue = guild.Timestamp;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Conflict(ex.ToString());
            }
 
            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<GuildSelectDTO>> InsertGuild(GuildInsertDTO dto)
        {
            var guild = dto.Adapt<Guild>();
            _context.Guilds.Add(guild);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetGuild), new { id = guild.GuildId }, guild.Adapt<GuildSelectDTO>());
        }

        // DELETE: api/Guilds/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteGuild(int id)
        {
            var guild = await _context.MultiGuilds.Where(g => g.GuildId == id).Include(g => g.Players).FirstOrDefaultAsync();
            if (guild == null)
            {
                return NotFound();
            }

            _context.MultiGuilds.Remove(guild);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GuildExists(int id)
        {
            return _context.Guilds.Any(e => e.GuildId == id);
        }
    }


    public class GuildInsertDTO : NamedEntity
    {
        public int? AdminPlayerId { get; set; }
    }

    public class GuildUpdateDTO : GuildInsertDTO
    {
        public int GuildId { get; set; }
        public byte[] Timestamp { get; set; }
    }

    public class GuildSelectDTO : GuildUpdateDTO
    {
        public DateTime Created { get; set; }
    }


}
