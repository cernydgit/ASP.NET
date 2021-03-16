using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.Mediator
{
    public class NotificationHandler : INotificationHandler<Notification>
    {
        public NotificationHandler(ILogger<NotificationHandler> logger)
        {
            Logger = logger;
        }

        public ILogger<NotificationHandler> Logger { get; }

        public Task Handle(Notification notification, CancellationToken cancellationToken)
        {
            Logger.LogInformation("Notification: " + notification.Messagee);
            return Task.CompletedTask;
        }
    }
}
