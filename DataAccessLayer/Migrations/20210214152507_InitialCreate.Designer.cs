﻿// <auto-generated />
using System;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataAccessLayer.Migrations
{
    [DbContext(typeof(ZorkDbContext))]
    [Migration("20210214152507_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DataAccessLayer.Models.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("playerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("playerId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("DataAccessLayer.Models.Monster", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Damages")
                        .HasColumnType("int");

                    b.Property<int?>("GameId")
                        .HasColumnType("int");

                    b.Property<int>("Hp")
                        .HasColumnType("int");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<double>("MissRate")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("Monsters");
                });

            modelBuilder.Entity("DataAccessLayer.Models.Object", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AttackStrengthBoost")
                        .HasColumnType("int");

                    b.Property<int>("DefenseBoost")
                        .HasColumnType("int");

                    b.Property<int?>("GameId")
                        .HasColumnType("int");

                    b.Property<int>("HPRestoreValue")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PlayerId")
                        .HasColumnType("int");

                    b.Property<int?>("PlayerId1")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("PlayerId");

                    b.HasIndex("PlayerId1");

                    b.ToTable("Objects");
                });

            modelBuilder.Entity("DataAccessLayer.Models.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Hp")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Xp")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Player");
                });

            modelBuilder.Entity("DataAccessLayer.Models.Weapon", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Damages")
                        .HasColumnType("int");

                    b.Property<int?>("GameId")
                        .HasColumnType("int");

                    b.Property<double>("MissRate")
                        .HasColumnType("float");

                    b.Property<int?>("PlayerId")
                        .HasColumnType("int");

                    b.Property<string>("name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("PlayerId");

                    b.ToTable("Weapons");
                });

            modelBuilder.Entity("DataAccessLayer.Models.Game", b =>
                {
                    b.HasOne("DataAccessLayer.Models.Player", "player")
                        .WithMany()
                        .HasForeignKey("playerId");

                    b.Navigation("player");
                });

            modelBuilder.Entity("DataAccessLayer.Models.Monster", b =>
                {
                    b.HasOne("DataAccessLayer.Models.Game", null)
                        .WithMany("Monsters")
                        .HasForeignKey("GameId");
                });

            modelBuilder.Entity("DataAccessLayer.Models.Object", b =>
                {
                    b.HasOne("DataAccessLayer.Models.Game", null)
                        .WithMany("Loots")
                        .HasForeignKey("GameId");

                    b.HasOne("DataAccessLayer.Models.Player", null)
                        .WithMany("Inventory")
                        .HasForeignKey("PlayerId");

                    b.HasOne("DataAccessLayer.Models.Player", null)
                        .WithMany("UsedObjects")
                        .HasForeignKey("PlayerId1");
                });

            modelBuilder.Entity("DataAccessLayer.Models.Weapon", b =>
                {
                    b.HasOne("DataAccessLayer.Models.Game", null)
                        .WithMany("Weapons")
                        .HasForeignKey("GameId");

                    b.HasOne("DataAccessLayer.Models.Player", null)
                        .WithMany("Weapons")
                        .HasForeignKey("PlayerId");
                });

            modelBuilder.Entity("DataAccessLayer.Models.Game", b =>
                {
                    b.Navigation("Loots");

                    b.Navigation("Monsters");

                    b.Navigation("Weapons");
                });

            modelBuilder.Entity("DataAccessLayer.Models.Player", b =>
                {
                    b.Navigation("Inventory");

                    b.Navigation("UsedObjects");

                    b.Navigation("Weapons");
                });
#pragma warning restore 612, 618
        }
    }
}