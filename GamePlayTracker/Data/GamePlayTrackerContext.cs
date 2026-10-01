using Microsoft.EntityFrameworkCore;
using GamePlayTracker.Models;

namespace GamePlayTracker.Data;

public class GamePlayTrackerContext : DbContext
{
    public GamePlayTrackerContext(DbContextOptions<GamePlayTrackerContext> options)
        : base(options)
    {
    }

    public DbSet<Game> Games => Set<Game>();

    protected override void OnModelCreating(ModelBuilder b)
    {
    }
}