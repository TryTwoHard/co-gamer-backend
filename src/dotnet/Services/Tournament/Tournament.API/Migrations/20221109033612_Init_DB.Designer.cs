﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tournament.API.Persistence;

#nullable disable

namespace Tournament.API.Migrations
{
    [DbContext(typeof(TournamentContext))]
    [Migration("20221109033612_Init_DB")]
    partial class Init_DB
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Tournament.API.Models.Entities.CompetitionFormat", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.ToTable("CompetitionFormat");
                });

            modelBuilder.Entity("Tournament.API.Models.Entities.GameCategory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.ToTable("GameCategory");
                });

            modelBuilder.Entity("Tournament.API.Models.Entities.TournamentEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTimeOffset>("BeginTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("CancelReason")
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<Guid>("CompetitionFormatId")
                        .HasColumnType("char(36)");

                    b.Property<DateTimeOffset>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<DateTimeOffset>("EndTime")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("GameCategoryId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("HostId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CompetitionFormatId");

                    b.HasIndex("GameCategoryId");

                    b.ToTable("Tournaments");
                });

            modelBuilder.Entity("Tournament.API.Models.Entities.TournamentEntity", b =>
                {
                    b.HasOne("Tournament.API.Models.Entities.CompetitionFormat", "CompetitionFormat")
                        .WithMany()
                        .HasForeignKey("CompetitionFormatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Tournament.API.Models.Entities.GameCategory", "GameCategory")
                        .WithMany()
                        .HasForeignKey("GameCategoryId");

                    b.Navigation("CompetitionFormat");

                    b.Navigation("GameCategory");
                });
#pragma warning restore 612, 618
        }
    }
}
