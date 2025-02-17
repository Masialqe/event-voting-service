﻿// <auto-generated />
using System;
using EVS.App.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EVS.App.Migrations.ApplicationDb
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250216145815_RowVersionAdded")]
    partial class RowVersionAdded
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("EVS.App.Domain.Events.Event", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("Event_Id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("Event_CreatedAt");

                    b.Property<string>("Description")
                        .HasColumnType("varchar(500)")
                        .HasColumnName("Event_Description");

                    b.Property<int>("EventState")
                        .HasColumnType("integer")
                        .HasColumnName("Event_State");

                    b.Property<int>("EventType")
                        .HasColumnType("integer")
                        .HasColumnName("Event_Type");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasColumnName("Event_Name");

                    b.Property<Guid>("VoterId")
                        .HasColumnType("uuid");

                    b.Property<int>("VoterLimit")
                        .HasColumnType("INTEGER")
                        .HasColumnName("Event_VoterLimit");

                    b.HasKey("Id");

                    b.HasIndex("VoterId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("EVS.App.Domain.VoterEvents.VoterEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("VoterEvent_Id");

                    b.Property<Guid>("EventId")
                        .HasColumnType("uuid");

                    b.Property<bool>("HasVoted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("VoterEvent_HasVoted");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("bytea")
                        .HasColumnName("VoterEvent_RowVersion");

                    b.Property<int>("Score")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0)
                        .HasColumnName("VoterEvent_Score");

                    b.Property<Guid>("VoterId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("VoterId");

                    b.ToTable("VoterEvent");
                });

            modelBuilder.Entity("EVS.App.Domain.Voters.Voter", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(254)")
                        .HasColumnName("Voter_Email");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasColumnName("Voter_UserId");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("Voter_Nickname");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Voters");
                });

            modelBuilder.Entity("EVS.App.Domain.Events.Event", b =>
                {
                    b.HasOne("EVS.App.Domain.Voters.Voter", "Voter")
                        .WithMany("CreatedEvents")
                        .HasForeignKey("VoterId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Voter");
                });

            modelBuilder.Entity("EVS.App.Domain.VoterEvents.VoterEvent", b =>
                {
                    b.HasOne("EVS.App.Domain.Events.Event", "Event")
                        .WithMany("VoterEvents")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EVS.App.Domain.Voters.Voter", "Voter")
                        .WithMany("VoterEvents")
                        .HasForeignKey("VoterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("Voter");
                });

            modelBuilder.Entity("EVS.App.Domain.Events.Event", b =>
                {
                    b.Navigation("VoterEvents");
                });

            modelBuilder.Entity("EVS.App.Domain.Voters.Voter", b =>
                {
                    b.Navigation("CreatedEvents");

                    b.Navigation("VoterEvents");
                });
#pragma warning restore 612, 618
        }
    }
}
