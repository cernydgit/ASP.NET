using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.Mediator
{
    public class NotifyGenericHandler<T> : INotificationHandler<T> 
        where T : /*Notify<T>,*/ INotification
    {
        private readonly ILogger<NotifyGenericHandler<T>> logger;

        public NotifyGenericHandler(ILogger<NotifyGenericHandler<T>> logger)
        {
            this.logger = logger;
        }

        public Task Handle(T notification, CancellationToken cancellationToken)
        {
            logger.LogInformation(notification.ToString());
            return Task.CompletedTask;
        }
    }


}


