using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Catalog.Entities
{
    public class GameDbContext : DbContext
    {
        public GameDbContext(DbContextOptions<GameDbContext> options) : base(options)
        {
        }

        public DbSet<Guild> Guilds { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Guild>().Property(b => b.Name).IsRequired();
            modelBuilder.Entity<Guild>().Property(b => b.Created).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Guild>().HasOne(g => g.Admin).WithOne().HasForeignKey<Guild>(g => g.AdminPlayerId);
            modelBuilder.Entity<Guild>().HasMany(g => g.Players).WithOne(p => p.Guild);
        }

    }

    public class NamedEntity
    {
        public string Name { get; set; } = Guid.NewGuid().ToString();
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

    public class Player : NamedEntity
    {
        public int PlayerId { get; set; }
        public int? GuildId { get; set; }

        [System.Text.Json.Serialization.JsonIgnore] [Newtonsoft.Json.JsonIgnore]
        public Guild Guild { get; set; }
    }

    public class Tag : NamedEntity
    {
        public int TagId { get; set; }

        [System.Text.Json.Serialization.JsonIgnore] [Newtonsoft.Json.JsonIgnore]
        public List<Guild> Guilds { get; set; } = new List<Guild>();
    }
}
