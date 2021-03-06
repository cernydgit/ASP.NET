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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Guild>().Property(b => b.Name).IsRequired();
            modelBuilder.Entity<Guild>().Property(b => b.Created).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Guild>().HasOne(g => g.Admin).WithOne().HasForeignKey<Guild>(g => g.AdminPlayerId);
            modelBuilder.Entity<Guild>().HasMany(g => g.Players).WithOne(p => p.Guild);
        }

    }


    public class Guild
    {
        public int GuildId { get; set; }
        public string Name { get; set; } = Guid.NewGuid().ToString();
        public DateTime Created { get; set; }
        public List<Player> Players { get; set; } = new List<Player>();
        public int?   AdminPlayerId { get; set; }
        public Player Admin { get; set; }
    }

    public class Player
    {
        public int PlayerId { get; set; }
        public string Name { get; set; } = Guid.NewGuid().ToString();
        public int? GuildId { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public Guild Guild { get; set; }
    }


}
