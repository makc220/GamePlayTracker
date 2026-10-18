using Microsoft.EntityFrameworkCore;
using GamePlayTracker.Models;

namespace GamePlayTracker.Data;

public class GamePlayTrackerContext : DbContext
{
     public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<Platform> Platforms => Set<Platform>();
    public GamePlayTrackerContext(DbContextOptions<GamePlayTrackerContext> options)
        : base(options)
    {
    }

    public DbSet<Game> Games => Set<Game>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Game>()
            .HasIndex(g => new { g.Title, g.ReleaseYear })
            .IsUnique();

        b.Entity<Game>()
            .HasOne(g => g.Genre)
            .WithMany()
            .HasForeignKey(g => g.GenreId)
            .OnDelete(DeleteBehavior.Restrict);

        b.Entity<Game>()
            .HasOne(g => g.Platform)
            .WithMany()
            .HasForeignKey(g => g.PlatformId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}