using MediatR;

namespace Catalog.Mediator
{
    public class Notify<T> : INotification
    {
        public T Value { get; set; }

        public override string ToString()
        {
            return base.ToString() + " " + Value.ToString();
        }
    }


}


