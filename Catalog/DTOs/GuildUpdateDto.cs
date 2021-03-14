using Catalog.Entities;

namespace Catalog.DTOs
{
    public class GuildUpdateDto : GuildInsertDto, IEntityID
    {
        public int GuildId { get; set; }
        public byte[] Timestamp { get; set; }
        int IEntityID.Id => GuildId;
    }


}
