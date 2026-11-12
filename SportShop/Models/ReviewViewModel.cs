using System.ComponentModel.DataAnnotations;

namespace SportShop.Models;

public class ReviewViewModel
{
    [Required(ErrorMessage = "Напишите отзыв")]
    [MinLength(10, ErrorMessage = "Отзыв должен быть не короче 10 символов")]
    [Display(Name = "Текст отзыва")]
    public string? Text { get; set; }

    [Range(1, 5, ErrorMessage = "Оценка от 1 до 5")]
    [Display(Name = "Оценка")]
    public int Rating { get; set; } = 5;
}