using Catalog.Entities;

namespace Catalog.DTOs
{
    public class GuildInsertDto : NamedEntity
    {
        public int? AdminPlayerId { get; set; }
    }


}
