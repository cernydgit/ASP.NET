using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Catalog.Mediator
{
    public class Tweet : IRequest<bool>
    { 
        public string Message { get; set; }
    }


}
