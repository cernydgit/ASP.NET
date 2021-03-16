using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.Mediator
{
    public class TweetHandler : IRequestHandler<Tweet, bool>
    {

        public TweetHandler(ILogger<TweetHandler> logger)
        {
            Logger = logger;
        }

        public ILogger<TweetHandler> Logger { get; }

        public Task<bool> Handle(Tweet request, CancellationToken cancellationToken)
        {
            Logger.LogInformation("Tweet: " + request.Message);
            return Task.FromResult(true);
        }
    }

}
