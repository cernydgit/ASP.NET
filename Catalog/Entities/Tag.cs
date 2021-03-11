using System.Collections.Generic;

namespace Catalog.Entities
{
    public class Tag : NamedEntity
    {
        public int TagId { get; set; }

        [System.Text.Json.Serialization.JsonIgnore] [Newtonsoft.Json.JsonIgnore]
        public List<Guild> Guilds { get; set; } = new List<Guild>();
    }
}
