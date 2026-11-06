namespace GamePlayTracker.Api;

public record GameDto(
    int Id,
    string Title,
    string Developer,
    string Genre,
    string Platform,
    DateOnly ReleaseDate,
    int ReleaseYear,
    string? Description);
