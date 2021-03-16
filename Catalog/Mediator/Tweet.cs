using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Mediator
{
    public class Tweet : IRequest<bool>
    { 
        public string Message { get; set; }
    }





}
