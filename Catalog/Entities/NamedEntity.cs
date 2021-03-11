using System;

namespace Catalog.Entities
{
    public class NamedEntity
    {
        public string Name { get; set; } = Guid.NewGuid().ToString();
    }
}
