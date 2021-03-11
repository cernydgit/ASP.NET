namespace Catalog.Entities
{
    public class Player : NamedEntity
    {
        public int PlayerId { get; set; }
        public int? GuildId { get; set; }

        [System.Text.Json.Serialization.JsonIgnore] [Newtonsoft.Json.JsonIgnore]
        public Guild Guild { get; set; }
    }
}
