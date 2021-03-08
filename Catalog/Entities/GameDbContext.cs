using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Catalog.Entities
{
    public class GameDbContext : DbContext
    {
        public GameDbContext(DbContextOptions<GameDbContext> options) : base(options)
        {
        }

        public DbSet<Guild> Guilds { get; set; }
        public DbSet<MultiGuild> MultiGuilds { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<MultiPlayer> MultiPlayers { get; set; }
        public DbSet<Tag> Tags { get; set; }
         
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Guild>().ToTable("Guilds");
            modelBuilder.Entity<Guild>().Property(b => b.Name).IsRequired();
            modelBuilder.Entity<Guild>().Property(b => b.Created).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Guild>().HasOne(g => g.Admin).WithOne().HasForeignKey<Guild>(g => g.AdminPlayerId);
            modelBuilder.Entity<Guild>().HasMany(g => g.Players).WithOne(p => p.Guild);

            modelBuilder.Entity<MultiGuild>().ToTable("MultiGuilds");
        }

        public IQueryable<GuildDetails> GetGuildDetails()
        {
            return Guilds.Include(g => g.Players).Select(g => new GuildDetails { GuildId = g.GuildId, PlayerCount = g.Players.Count() });
        }

    }

    public class NamedEntity
    {
        public string Name { get; set; } = Guid.NewGuid().ToString();
    }


    public class GuildDetails
    {
        public int GuildId { get; set; }
        public int PlayerCount { get; set; }
    }

    public class Guild : NamedEntity
    {
        public int GuildId { get; set; }
        public DateTime Created { get; set; }
        public List<Player> Players { get; set; } = new List<Player>();
        public int?   AdminPlayerId { get; set; }
        public Player Admin { get; set; }
        public List<Tag> Tags { get; set; } = new List<Tag>();
    }


    public class MultiGuild : Guild
    {
        public int MMR { get; set; }
    }

    public class Player : NamedEntity
    {
        public int PlayerId { get; set; }
        public int? GuildId { get; set; }

        [System.Text.Json.Serialization.JsonIgnore] [Newtonsoft.Json.JsonIgnore]
        public Guild Guild { get; set; }
    }

    public class MultiPlayer : Player
    {
        public int MMR { get; set; }
    }

    public class Tag : NamedEntity
    {
        public int TagId { get; set; }

        [System.Text.Json.Serialization.JsonIgnore] [Newtonsoft.Json.JsonIgnore]
        public List<Guild> Guilds { get; set; } = new List<Guild>();
    }
}
