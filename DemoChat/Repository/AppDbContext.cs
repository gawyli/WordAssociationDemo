using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoChat.Audio.Models;
using DemoChat.Chat.Models;
using DemoChat.Emotions.Models;
using DemoChat.Games.Models;
using DemoChat.Hume.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel.ChatCompletion;

namespace DemoChat.Repository;
public class AppDbContext : DbContext
{
    public DbSet<ChatSession> ChatSessions { get; set; }
    public DbSet<AudioFile> AudioFiles { get; set; }

    public DbSet<GameSession> GameSessions { get; set; }
    public DbSet<Association> Associations { get; set; }

    public DbSet<EmotionsSession> EmotionsSessions { get; set; }

    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChatSession>().HasKey(x => x.Id);

        modelBuilder.Entity<AudioFile>().HasKey(x => x.Id);
        modelBuilder.Entity<AudioFile>().Property(x => x.AuthorRole)
            .HasConversion(v => v.ToString(), v => new AuthorRole(v)); //this is not work - i think

        modelBuilder.Entity<Association>().HasKey(x => x.Id);
        modelBuilder.Entity<Association>().Property(a => a.Id).ValueGeneratedOnAdd();

        modelBuilder.Entity<EmotionsSession>().HasKey(x => x.Id);
        
        base.OnModelCreating(modelBuilder);
    }
}
