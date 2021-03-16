using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.Mediator
{
    public class GeneralNotificationHandler : INotificationHandler<INotification>
    {
        public GeneralNotificationHandler(ILogger<GeneralNotificationHandler> logger)
        {
            Logger = logger;
        }

        public ILogger<GeneralNotificationHandler> Logger { get; }

        public Task Handle(INotification notification, CancellationToken cancellationToken)
        {
            Logger.LogInformation("Notification: " + notification.ToString());
            return Task.CompletedTask;
        }
    }


}
