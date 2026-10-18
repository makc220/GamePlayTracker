using System.ComponentModel.DataAnnotations;

namespace GamePlayTracker.Models;

public class Genre
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Зазначте назву жанру")]
    [StringLength(100, ErrorMessage = "Назва жанру не може перевищувати 100 символів")]
    public string Name { get; set; } = "";
}