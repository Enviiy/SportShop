using System.ComponentModel.DataAnnotations;

namespace SportShop.Domain.Entities;

public abstract class EntityBase
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Заполните название")]
    [Display(Name = "Название")]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
}