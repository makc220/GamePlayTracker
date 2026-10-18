using System.ComponentModel.DataAnnotations;

namespace GamePlayTracker.Models;

public class Platform
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Зазначте назву платформи")]
    [StringLength(100, ErrorMessage = "Назва платформи не може перевищувати 100 символів")]
    public string Name { get; set; } = "";
}