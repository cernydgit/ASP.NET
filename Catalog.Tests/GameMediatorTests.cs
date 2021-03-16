using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Catalog.Tests.Client;
using MediatR;
using Catalog.Mediator;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Catalog.Tests
{
    public class GameMediatorTests 
    {
        [Test]
        public async Task Test()
        {
            var factory = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    Handlers<int>.Register(services);
                });
            });
            var mediator = factory.Services.GetService<IMediator>();
            var i = await mediator.Send(new DummyRequest<int, int> { Input = 3 });
            var s = await mediator.Send(new DummyRequest<int, string> { Input = 4 });
        }
    }
}