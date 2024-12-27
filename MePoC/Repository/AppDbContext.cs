using MePoC.DataBus;
using MePoC.Models;
using Microsoft.EntityFrameworkCore;

namespace MePoC.Repository;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Record> Records { get; set; }
    public DbSet<Response> Responses { get; set; }
    public DbSet<Memories> Memories { get; set; }
    public DbSet<Word> Words { get; set; }
    public DbSet<WordsPair> WordsPairs { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<JobDetails> JobDetails { get; set; }
    public DbSet<ZipDetails> ZipDetails { get; set; }
    public DbSet<TestDetails> TestDetails { get; set; }

    // TODO: HumeAI
    //public DbSet<Reaction> Reactions { get; set; }
    //public DbSet<Indicator> Indicators { get; set; }
    //public DbSet<EmotionalReaction> EmotionalReactions { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Record>().HasKey(x => x.Id);
        modelBuilder.Entity<Record>().Property(x => x.Name).IsRequired();
        modelBuilder.Entity<Record>().Property(x => x.Path).IsRequired();
        modelBuilder.Entity<Record>().Property(x => x.Created).IsRequired();

        modelBuilder.Entity<Response>().HasKey(x => x.Id);
        modelBuilder.Entity<Response>().Property(x => x.Text).IsRequired();
        modelBuilder.Entity<Response>().Property(x => x.AudioName).IsRequired();
        modelBuilder.Entity<Response>().Property(x => x.AudioPath).IsRequired();
        modelBuilder.Entity<Response>().Property(x => x.Created).IsRequired();

        modelBuilder.Entity<ZipDetails>().HasKey(x => x.Id);

        modelBuilder.Entity<JobDetails>().HasKey(x => x.Id);

        modelBuilder.Entity<Memories>().HasKey(x => x.Id);
        modelBuilder.Entity<Memories>().Property(x => x.ContextHistory).IsRequired();
        modelBuilder.Entity<Memories>().Property(x => x.Created).IsRequired();

        modelBuilder.Entity<Word>().HasKey(x => x.Id);
        modelBuilder.Entity<Word>().Property(x => x.Content).IsRequired();
        modelBuilder.Entity<Word>().Property(x => x.Type).IsRequired();

        modelBuilder.Entity<WordsPair>().HasKey(x => x.Id);

        // TODO: Tech debt - OnDelete(DeleteBehavior.SetNull)
        modelBuilder.Entity<WordsPair>().HasOne(x => x.Stimulus)
            .WithMany()
            .HasForeignKey(x => x.StimulusId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        modelBuilder.Entity<WordsPair>().HasOne(x => x.Response)
            .WithMany()
            .HasForeignKey(x => x.ResponseId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        modelBuilder.Entity<WordsPair>().Property(x => x.ResponseTime).IsRequired();
        modelBuilder.Entity<WordsPair>().HasOne(x => x.Session)
            .WithMany(s => s.WordsPairs)    
            .HasForeignKey(x => x.SessionId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        //modelBuilder.Entity<WordsPair>().HasMany(x => x.ComplexIndicators).WithOne(x => x.WordsPair);

            
        modelBuilder.Entity<Session>().Property(x => x.Created).IsRequired();
        modelBuilder.Entity<Session>().Property(x => x.StartTime).IsRequired();

        modelBuilder.Entity<TestDetails>().HasKey(x => x.Id);
        modelBuilder.Entity<TestDetails>().HasOne(x => x.Session)
            .WithMany()
            .HasForeignKey(x => x.SessionId)
            .OnDelete(DeleteBehavior.Restrict);


        //modelBuilder.Entity<Reaction>().HasKey(x => x.Id);

        //modelBuilder.Entity<Indicator>().HasKey(x => x.Id);
        //modelBuilder.Entity<Indicator>().HasOne(x => x.WordsPair.Id);

        //modelBuilder.Entity<EmotionalReaction>().HasKey(x => x.Id);

    }
}
