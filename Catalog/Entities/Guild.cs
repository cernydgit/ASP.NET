using System;
using System.Collections.Generic;

namespace Catalog.Entities
{
    public class Guild : NamedEntity
    {
        public int GuildId { get; set; }
        public DateTime Created { get; set; }
        public List<Player> Players { get; set; } = new List<Player>();
        public int?   AdminPlayerId { get; set; }
        public Player Admin { get; set; }
        public List<Tag> Tags { get; set; } = new List<Tag>();
        public byte[] Timestamp { get; set; }
    }
}
