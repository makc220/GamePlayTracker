using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using GamePlayTracker.Data;
using GamePlayTracker.Models;
using GamePlayTracker.Services;

namespace GamePlayTracker.Tests;

public class SummaryCalculatorTests : IDisposable
{
    private readonly SqliteConnection connection;
    private readonly GamePlayTrackerContext db;

    public SummaryCalculatorTests()
    {
        connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<GamePlayTrackerContext>()
            .UseSqlite(connection)
            .Options;

        db = new GamePlayTrackerContext(options);
        db.Database.EnsureCreated();
    }

    public void Dispose()
    {
        db.Dispose();
        connection.Dispose();
    }

    private Game CreateGame(string title, int year, Platform platform, Genre genre)
    {
        return new Game
        {
            Title = title,
            TitleSearch = title.ToLower(),
            Developer = "Test Developer",
            ReleaseDate = new DateOnly(year, 1, 1),
            ReleaseYear = year,
            Platform = platform,
            Genre = genre
        };
    }

    [Fact]
    public async Task ByPlatformForEmptySetReturnsNoRows()
    {
        var result = await SummaryCalculator.ByPlatformAsync(db.Games);

        Assert.Empty(result);
    }

    [Fact]
    public async Task ByPlatformGroupsGamesWithSamePlatformIntoOneRow()
    {
        var platform = new Platform { Name = "PC" };
        var genre = new Genre { Name = "Action" };

        db.Games.AddRange(
            CreateGame("Game One", 2020, platform, genre),
            CreateGame("Game Two", 2021, platform, genre));

        await db.SaveChangesAsync();

        var result = await SummaryCalculator.ByPlatformAsync(db.Games);

        var row = Assert.Single(result);
        Assert.Equal("PC", row.Name);
        Assert.Equal(2, row.Count);
    }

    [Fact]
    public async Task ByPlatformOrdersRowsByGameCountDescending()
    {
        var pc = new Platform { Name = "PC" };
        var xbox = new Platform { Name = "Xbox" };
        var genre = new Genre { Name = "Action" };

        db.Games.AddRange(
            CreateGame("Game One", 2020, pc, genre),
            CreateGame("Game Two", 2021, pc, genre),
            CreateGame("Game Three", 2022, pc, genre),
            CreateGame("Game Four", 2020, xbox, genre));

        await db.SaveChangesAsync();

        var result = await SummaryCalculator.ByPlatformAsync(db.Games);

        Assert.Equal(2, result.Count);
        Assert.Equal("PC", result[0].Name);
        Assert.Equal(3, result[0].Count);
        Assert.Equal("Xbox", result[1].Name);
        Assert.Equal(1, result[1].Count);
    }

    [Fact]
    public async Task ByYearGroupsGamesIntoSeparateRows()
    {
        var platform = new Platform { Name = "PC" };
        var genre = new Genre { Name = "Action" };

        db.Games.AddRange(
            CreateGame("Game One", 2020, platform, genre),
            CreateGame("Game Two", 2020, platform, genre),
            CreateGame("Game Three", 2022, platform, genre));

        await db.SaveChangesAsync();

        var result = await SummaryCalculator.ByYearAsync(db.Games);

        Assert.Equal(2, result.Count);
        Assert.Equal("2020", result[0].Name);
        Assert.Equal(2, result[0].Count);
        Assert.Equal("2022", result[1].Name);
        Assert.Equal(1, result[1].Count);
    }

    [Fact]
    public async Task TotalReturnsNumberOfGames()
    {
        var platform = new Platform { Name = "PC" };
        var genre = new Genre { Name = "Action" };

        db.Games.AddRange(
            CreateGame("Game One", 2020, platform, genre),
            CreateGame("Game Two", 2021, platform, genre),
            CreateGame("Game Three", 2022, platform, genre));

        await db.SaveChangesAsync();

        var result = await SummaryCalculator.TotalAsync(db.Games);

        Assert.Equal("Усього", result.Name);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task ByPlatformForSingleGameReturnsOneRow()
    {
        var platform = new Platform { Name = "PC" };
        var genre = new Genre { Name = "Action" };

        db.Games.Add(
            CreateGame("Game One", 2020, platform, genre));

        await db.SaveChangesAsync();

        var result = await SummaryCalculator.ByPlatformAsync(db.Games);

        var row = Assert.Single(result);
        Assert.Equal("PC", row.Name);
        Assert.Equal(1, row.Count);
    }
}
