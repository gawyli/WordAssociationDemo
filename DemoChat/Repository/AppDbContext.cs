using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoChat.Audio.Models;
using DemoChat.Chat.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel.ChatCompletion;

namespace DemoChat.Repository;
public class AppDbContext : DbContext
{
    public DbSet<ChatSession> ChatSessions { get; set; }
    public DbSet<AudioFile> AudioFiles { get; set; }

    public AppDbContext(DbContextOptions options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChatSession>().HasKey(x => x.Id);
        modelBuilder.Entity<ChatSession>().HasMany(x => x.AudioFiles).WithOne().HasForeignKey(x => x.ChatSessionId);

        modelBuilder.Entity<AudioFile>().HasKey(x => x.Id);
        modelBuilder.Entity<AudioFile>().Property(x => x.AuthorRole)
            .HasConversion(v => v.ToString(), v => (AuthorRole)Enum.Parse(typeof(AuthorRole), v));
    }
}
