using GamePlayTracker.Components;
using Microsoft.EntityFrameworkCore;
using GamePlayTracker.Data;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextFactory<GamePlayTrackerContext>(o =>
    o.UseSqlite("Data Source=gameplaytracker.db"));

builder.Services.AddQuickGridEntityFrameworkAdapter();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddOpenApi();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapGet("/api/games", async (
    IDbContextFactory<GamePlayTrackerContext> dbFactory) =>
{
    await using var db = await dbFactory.CreateDbContextAsync();

    var games = await db.Games
        .Include(g => g.Genre)
        .Include(g => g.Platform)
        .OrderBy(g => g.Title)
        .Select(g => new GamePlayTracker.Api.GameDto(
            g.Id,
            g.Title,
            g.Developer,
            g.Genre != null ? g.Genre.Name : "",
            g.Platform != null ? g.Platform.Name : "",
            g.ReleaseDate,
            g.ReleaseYear,
            g.Description))
        .ToListAsync();

    return TypedResults.Ok(games);
});

app.MapGet("/api/games/{id:int}", async Task<IResult> (
    int id,
    IDbContextFactory<GamePlayTrackerContext> dbFactory) =>
{
    await using var db = await dbFactory.CreateDbContextAsync();

    var game = await db.Games
        .Where(g => g.Id == id)
        .Select(g => new GamePlayTracker.Api.GameDto(
            g.Id,
            g.Title,
            g.Developer,
            g.Genre != null ? g.Genre.Name : "",
            g.Platform != null ? g.Platform.Name : "",
            g.ReleaseDate,
            g.ReleaseYear,
            g.Description))
        .FirstOrDefaultAsync();

    if (game is null)
    {
        return TypedResults.NotFound();
    }

    return TypedResults.Ok(game);
});

app.MapGet("/api/summary", async (
    IDbContextFactory<GamePlayTrackerContext> dbFactory) =>
{
    await using var db = await dbFactory.CreateDbContextAsync();

    var total = await db.Games.CountAsync();
    var platforms = await GamePlayTracker.Services.SummaryCalculator
        .ByPlatformAsync(db.Games);

    return TypedResults.Ok(new
    {
        TotalGames = total,
        GamesByPlatform = platforms
    });
});

app.Run();
