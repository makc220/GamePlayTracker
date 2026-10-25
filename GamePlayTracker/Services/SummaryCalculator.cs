
using Microsoft.EntityFrameworkCore;
using GamePlayTracker.Models;

namespace GamePlayTracker.Services;

public record SummaryRow(string Name, int Count);

public static class SummaryCalculator
{
    public static async Task<List<SummaryRow>> ByPlatformAsync(
        IQueryable<Game> query) =>
        await query
            .GroupBy(g => g.Platform!.Name)
            .OrderByDescending(g => g.Count())
            .Select(g => new SummaryRow(
                g.Key,
                g.Count()))
            .ToListAsync();

    public static async Task<List<SummaryRow>> ByYearAsync(
        IQueryable<Game> query) =>
        await query
            .GroupBy(g => g.ReleaseYear)
            .OrderBy(g => g.Key)
            .Select(g => new SummaryRow(
                g.Key.ToString(),
                g.Count()))
            .ToListAsync();

    public static async Task<SummaryRow> TotalAsync(
        IQueryable<Game> query) =>
        new SummaryRow(
            "Усього",
            await query.CountAsync());
}
