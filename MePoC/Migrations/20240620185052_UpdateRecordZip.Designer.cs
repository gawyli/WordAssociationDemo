﻿// <auto-generated />
using System;
using MePoC.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MePoC.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240620185052_UpdateRecordZip")]
    partial class UpdateRecordZip
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MePoC.Models.Memories", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ContextHistory")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Memories");
                });

            modelBuilder.Entity("MePoC.Models.Record", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsProcessed")
                        .HasColumnType("bit");

                    b.Property<string>("JobId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Records");
                });

            modelBuilder.Entity("MePoC.Models.Response", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AudioName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AudioPath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Responses");
                });

            modelBuilder.Entity("MePoC.Models.Session", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("History")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Reliablitiy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StopTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Validity")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("MePoC.Models.Word", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Words");
                });

            modelBuilder.Entity("MePoC.Models.WordsPair", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ResponseId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("ResponseTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("SessionId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("StimulusId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ResponseId");

                    b.HasIndex("SessionId");

                    b.HasIndex("StimulusId");

                    b.ToTable("WordsPairs");
                });

            modelBuilder.Entity("MePoC.Models.WordsPair", b =>
                {
                    b.HasOne("MePoC.Models.Word", "Response")
                        .WithMany()
                        .HasForeignKey("ResponseId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("MePoC.Models.Session", "Session")
                        .WithMany("WordsPairs")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MePoC.Models.Word", "Stimulus")
                        .WithMany()
                        .HasForeignKey("StimulusId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Response");

                    b.Navigation("Session");

                    b.Navigation("Stimulus");
                });

            modelBuilder.Entity("MePoC.Models.Session", b =>
                {
                    b.Navigation("WordsPairs");
                });
#pragma warning restore 612, 618
        }
    }
}
