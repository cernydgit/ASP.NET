using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Catalog.Entities;
using Mapster;
using System.Linq;

namespace Catalog.Controllers
{
    public class CrudControllerBase<TContext> : ControllerBase where TContext : DbContext
    {
        protected TContext _context;

        public CrudControllerBase(TContext context)
        {
            _context = context;
        }

        protected virtual async Task<ActionResult<IEnumerable<TDto>>> Get<TEntity, TDto>() where TEntity : class
        {
            return await _context.Set<TEntity>().ProjectToType<TDto>().ToListAsync();
        }

        protected virtual async Task<ActionResult<TDto>> Get<TEntity, TDto>(int id) where TEntity : class
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);

            return entity == null ? NotFound() : entity.Adapt<TDto>();
        }

        protected virtual async Task<IActionResult> Update<TEntity, TDto>(int id, TDto dto)
            where TEntity : class, IEntityID
            where TDto : IEntityID
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            var entity = await _context.Set<TEntity>().FindAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            dto.Adapt(entity);

            if (entity is IEntityTimestamp timestamp)
            {
                _context.Entry(entity).Property(nameof(timestamp.Timestamp)).OriginalValue = timestamp.Timestamp;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                //TODO: model state dictionary
                return Conflict(ex.ToString());
            }

            return NoContent();
        }


        protected virtual async Task<ActionResult<TReturnDto>> Insert<TEntity, TInsertDto, TReturnDto>(TInsertDto dto, string getMethod) where TEntity : class, IEntityID
        {
            var entity = dto.Adapt<TEntity>();
            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync();
            return CreatedAtAction(getMethod, new { id = entity.Id }, entity.Adapt<TReturnDto>());
        }

        protected virtual async Task<IActionResult> DeleteOne<TEntity>(IQueryable<TEntity> deleteQuery) where TEntity : class
        {
            var entity = await deleteQuery.FirstOrDefaultAsync();
            if (entity == null)
            {
                return NotFound();
            }

            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
