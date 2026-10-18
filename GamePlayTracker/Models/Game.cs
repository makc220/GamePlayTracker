
using System.ComponentModel.DataAnnotations;

namespace GamePlayTracker.Models;

public class Game
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Зазначте назву гри")]
    [StringLength(200, ErrorMessage = "Назва гри не може перевищувати 200 символів")]
    public string Title { get; set; } = "";

    public int GenreId { get; set; }
    public Genre? Genre { get; set; }

    [Required(ErrorMessage = "Зазначте розробника гри")]
    [StringLength(150, ErrorMessage = "Назва розробника не може перевищувати 150 символів")]
    public string Developer { get; set; } = "";
    public int PlatformId { get; set; }
    public Platform? Platform { get; set; }

    public DateOnly ReleaseDate { get; set; }

    [Range(1970, 2100, ErrorMessage = "Рік випуску має бути від 1970 до 2100")]
    public int ReleaseYear { get; set; }

    [StringLength(2000, ErrorMessage = "Опис не може перевищувати 2000 символів")]
    public string? Description { get; set; }
}