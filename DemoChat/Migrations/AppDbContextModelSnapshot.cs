﻿// <auto-generated />
using System;
using DemoChat.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DemoChat.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.2");

            modelBuilder.Entity("DemoChat.Audio.Models.AudioFile", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("AuthorRole")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ChatSessionId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Content")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<bool?>("IsProcessed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("MimeType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ChatSessionId");

                    b.ToTable("AudioFiles");
                });

            modelBuilder.Entity("DemoChat.Chat.Models.ChatSession", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ChatHistory")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ChatSessions");
                });

            modelBuilder.Entity("DemoChat.Audio.Models.AudioFile", b =>
                {
                    b.HasOne("DemoChat.Chat.Models.ChatSession", null)
                        .WithMany("AudioFiles")
                        .HasForeignKey("ChatSessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DemoChat.Chat.Models.ChatSession", b =>
                {
                    b.Navigation("AudioFiles");
                });
#pragma warning restore 612, 618
        }
    }
}
