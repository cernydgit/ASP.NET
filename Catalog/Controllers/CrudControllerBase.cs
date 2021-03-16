﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Catalog.Entities;
using Mapster;
using System.Linq;
using Microsoft.Extensions.Logging;
using MapsterMapper;
using System.ComponentModel.DataAnnotations;
using MediatR;
using Catalog.Mediator;
using System;

namespace Catalog.Controllers
{
    public class CrudControllerBase<TContext> : ControllerBase where TContext : DbContext
    {
        protected TContext Context { get; }
        protected ILogger Logger { get; }
        protected IMapper Mapper { get; }
        protected IMediator Mediator { get; }

        public CrudControllerBase(TContext context, IMapper mapper, IMediator mediator, ILogger logger)
        {
            Context = context;
            Logger = logger;
            Mapper = mapper;
            Mediator = mediator;
        }

        protected virtual async Task<ActionResult<IEnumerable<TDto>>> Get<TEntity, TDto>() where TEntity : class
        {
            await Mediator.Publish(new Notification { Messagee = "GetNotification" });
            var ret = await Mediator.Send(new Tweet { Message = "GetTweet" });
            return await Context.Set<TEntity>().ProjectToType<TDto>(Mapper.Config).ToListAsync();
        }

        protected virtual async Task<ActionResult<IEnumerable<TDto>>> GetPage<TEntity, TDto>(int skip, int take) where TEntity : class
        {
            return await Context.Set<TEntity>().Skip(skip).Take(take).ProjectToType<TDto>(Mapper.Config).ToListAsync();
        }

        protected virtual async Task<ActionResult<TDto>> Get<TEntity, TDto>(int id) where TEntity : class
        {
            //await Mediator.Publish(new Notify<string> { Value = "hi1." });
            //await Mediator.Publish(new Notification { Messagee = "hi2." });
            //var entity = await Context.Set<TEntity>().FindAsync(id);
            var b = await Mediator.Send(new DummyBoolRequest<string> { Input = "input1" });
            var s = await Mediator.Send(new DummyStringRequest());
            var entity = await Mediator.Send(new GetRequest<TEntity> { Id = id });

            return entity == null ? NotFound() : Mapper.Map<TDto>(entity);
        }

        protected virtual async Task<IActionResult> Update<TEntity, TDto>(int id, TDto dto)
            where TEntity : class, IEntityID
            where TDto : IEntityID
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            var entity = await Context.Set<TEntity>().FindAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            Mapper.Map(dto, entity);

            if (entity is IEntityTimestamp timestamp)
            {
                Context.Entry(entity).Property(nameof(timestamp.Timestamp)).OriginalValue = timestamp.Timestamp;
            }

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Logger.LogWarning($"DbUpdateConcurrencyException: {entity}, id:{id}, Exception:\n{ex}");
                return Conflict(); //TODO: model state dictionary
            }

            return NoContent();
        }


        protected virtual async Task<ActionResult<TReturnDto>> Insert<TEntity, TInsertDto, TReturnDto>(TInsertDto dto, string getMethod) where TEntity : class, IEntityID
        {
            var entity = Mapper.Map<TEntity>(dto);
            Context.Set<TEntity>().Add(entity);
            await Context.SaveChangesAsync();
            return CreatedAtAction(getMethod, new { id = entity.Id }, Mapper.Map<TReturnDto>(entity));
        }

        protected virtual async Task<IActionResult> DeleteOne<TEntity>(IQueryable<TEntity> deleteQuery) where TEntity : class
        {
            var entity = await deleteQuery.FirstOrDefaultAsync();
            if (entity == null)
            {
                return NotFound();
            }

            Context.Set<TEntity>().Remove(entity);
            await Context.SaveChangesAsync();
            return NoContent();
        }
    }
}
