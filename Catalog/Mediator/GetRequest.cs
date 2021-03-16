using MediatR;

namespace Catalog.Mediator
{
    public class GetRequest<TEntity> : IRequest<TEntity>
    {
        public object Id { get; set; }
    }


}
