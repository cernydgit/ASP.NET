using MediatR;

namespace Catalog.Mediator
{
    public class Notification : INotification
    {
        public string Messagee { get; set; }
    }



}
