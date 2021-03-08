﻿// <auto-generated />
using System;
using Catalog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Catalog.Migrations
{
    [DbContext(typeof(GameDbContext))]
    partial class GameDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Catalog.Entities.Guild", b =>
                {
                    b.Property<int>("GuildId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AdminPlayerId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("GuildId");

                    b.HasIndex("AdminPlayerId")
                        .IsUnique()
                        .HasFilter("[AdminPlayerId] IS NOT NULL");

                    b.ToTable("Guilds");
                });

            modelBuilder.Entity("Catalog.Entities.Player", b =>
                {
                    b.Property<int>("PlayerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("GuildId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PlayerId");

                    b.HasIndex("GuildId");

                    b.ToTable("Players");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Player");
                });

            modelBuilder.Entity("Catalog.Entities.Tag", b =>
                {
                    b.Property<int>("TagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TagId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("GuildTag", b =>
                {
                    b.Property<int>("GuildsGuildId")
                        .HasColumnType("int");

                    b.Property<int>("TagsTagId")
                        .HasColumnType("int");

                    b.HasKey("GuildsGuildId", "TagsTagId");

                    b.HasIndex("TagsTagId");

                    b.ToTable("GuildTag");
                });

            modelBuilder.Entity("Catalog.Entities.MultiGuild", b =>
                {
                    b.HasBaseType("Catalog.Entities.Guild");

                    b.Property<int>("MMR")
                        .HasColumnType("int");

                    b.ToTable("MultiGuilds");
                });

            modelBuilder.Entity("Catalog.Entities.MultiPlayer", b =>
                {
                    b.HasBaseType("Catalog.Entities.Player");

                    b.Property<int>("MMR")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("MultiPlayer");
                });

            modelBuilder.Entity("Catalog.Entities.Guild", b =>
                {
                    b.HasOne("Catalog.Entities.Player", "Admin")
                        .WithOne()
                        .HasForeignKey("Catalog.Entities.Guild", "AdminPlayerId");

                    b.Navigation("Admin");
                });

            modelBuilder.Entity("Catalog.Entities.Player", b =>
                {
                    b.HasOne("Catalog.Entities.Guild", "Guild")
                        .WithMany("Players")
                        .HasForeignKey("GuildId");

                    b.Navigation("Guild");
                });

            modelBuilder.Entity("GuildTag", b =>
                {
                    b.HasOne("Catalog.Entities.Guild", null)
                        .WithMany()
                        .HasForeignKey("GuildsGuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Catalog.Entities.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsTagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Catalog.Entities.MultiGuild", b =>
                {
                    b.HasOne("Catalog.Entities.Guild", null)
                        .WithOne()
                        .HasForeignKey("Catalog.Entities.MultiGuild", "GuildId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Catalog.Entities.Guild", b =>
                {
                    b.Navigation("Players");
                });
#pragma warning restore 612, 618
        }
    }
}
