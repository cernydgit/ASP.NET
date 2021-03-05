using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Catalog.Entities
{
    public class GameDbContext : DbContext
    {
        public GameDbContext(DbContextOptions<GameDbContext> options) : base(options)
        {
        }

        public DbSet<Guild> Guilds { get; set; }
        public DbSet<GuildsView> GuildsViews { get; set; }
        public DbSet<Player> Players { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Guild>().Property(b => b.Name).IsRequired();
            modelBuilder.Entity<Guild>().Property(b => b.Created).HasDefaultValueSql("getdate()");

            modelBuilder.Entity<GuildsView>().ToView("GuildsView").HasKey(g => g.GuildId);
        }

    }


    public class Guild
    {
        public int GuildId { get; set; }
        public string Name { get; set; } = Guid.NewGuid().ToString();
        public DateTime Created { get; set; }
        public List<Player> Players { get; set; } = new List<Player>();
    }

    public class GuildsView
    {
        public int GuildId { get; set; }
        public string Name { get; set; }
        public int PlayerCount { get; set; }
    }


    public class Player
    {
        public int PlayerId { get; set; }
        public string Name { get; set; } = Guid.NewGuid().ToString();
        public int GuildId { get; set; }

        //[JsonIgnore]
        //public Guild Guild { get; set; }
    }

    //public record Guild(int GuildId = default, string Name = default, DateTime Created = default)
    //{ 
    //    public List<Player> Players { get; init;  } = new List<Player>();
    //}

    //public record Player(int PlayerId = 0, string Name = null, int? GuildId = null);

}
