using System.ComponentModel.DataAnnotations;

namespace SportShop.Models;

public class RegisterViewModel
{
    [Required, EmailAddress]
    [Display(Name = "Email")]
    public string? Email { get; set; }

    [Required, MinLength(6)]
    [UIHint("password")]
    [Display(Name = "Пароль")]
    public string? Password { get; set; }

    [Required, Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
    [UIHint("password")]
    [Display(Name = "Повторите пароль")]
    public string? PasswordConfirm { get; set; }
}