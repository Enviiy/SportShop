using System.ComponentModel.DataAnnotations;

namespace SportShop.Models;

public class CheckoutViewModel
{
    [Required(ErrorMessage = "Укажите имя")]
    [Display(Name = "Имя получателя")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Укажите телефон")]
    [Phone]
    [Display(Name = "Телефон")]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Укажите адрес")]
    [Display(Name = "Адрес доставки")]
    public string? Address { get; set; }
}