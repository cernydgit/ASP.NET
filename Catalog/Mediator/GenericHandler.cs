using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.Mediator
{

    public class DummyRequest<TInput, TResponse> : IRequest<TResponse>
    {
        public TInput Input { get; set; }
        public override string ToString()
        {
            return Input.ToString();
        }

    }

    public class GenericHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(default(TResponse));
        }
    }

    public class Handlers<TEntity>
    {
        public class ToIntHandler : GenericHandler<DummyRequest<TEntity,int>, int> { }
        public class ToStringHandler : GenericHandler<DummyRequest<TEntity, string>, string> { }

        public static void Register(IServiceCollection services)
        {
            services.AddSingleton<IRequestHandler<DummyRequest<TEntity, int>, int>, ToIntHandler>();
            services.AddSingleton<IRequestHandler<DummyRequest<TEntity, string>, string>, ToStringHandler>();
        }
    }

    


}


