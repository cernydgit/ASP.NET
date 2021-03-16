using Catalog.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.Mediator
{
    public class GetHandler<TContext, TEntity> : IRequestHandler<GetRequest<TEntity>, TEntity>
        where TContext : DbContext
        where TEntity : class
    {
        private readonly TContext context;

        public GetHandler(TContext context)
        {
            this.context = context;
        }
        public async Task<TEntity> Handle(GetRequest<TEntity> request, CancellationToken cancellationToken)
        {
            return await context.Set<TEntity>().FindAsync(request.Id);
        }
    }


    public class GetGameHandler<TEntity> : GetHandler<GameDbContext, TEntity> where TEntity : class
    {
        public GetGameHandler(GameDbContext context) : base(context) { }
    }


    public class GetGuildHandler : GetGameHandler<Guild>
    {
        public GetGuildHandler(GameDbContext context) : base(context) { }
    }

    public class GetGenericHandler<TEntity> : IRequestHandler<GetRequest<TEntity>, TEntity>
    {
        public Task<TEntity> Handle(GetRequest<TEntity> request, CancellationToken cancellationToken)
        {
            return Task.FromResult(default(TEntity));
        }
    }


}


