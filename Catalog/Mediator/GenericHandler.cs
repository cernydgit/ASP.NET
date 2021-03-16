using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.Mediator
{
    public class DummyBoolRequest<T> : IRequest<bool> { public T Input { get; set; }}
    public class DummyStringRequest : IRequest<string> { }

    public class GenericHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(default(TResponse));
        }
    }

    public class Handlers
    {
        public class DummyBoolHandler : GenericHandler<DummyBoolRequest<string>, bool> { }

        public class DummyStringHandler : GenericHandler<DummyStringRequest, string> { }
    }
}


