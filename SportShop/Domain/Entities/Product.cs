using System.ComponentModel.DataAnnotations;
using SportShop.Domain.Enums;

namespace SportShop.Domain.Entities;

public class Product : EntityBase
{
    [Display(Name = "Категория")]
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }

    [Display(Name = "Бренд")]
    public int? BrandId { get; set; }
    public Brand? Brand { get; set; }

    [Display(Name = "Краткое описание")]
    [MaxLength(3000)]
    public string? ShortDescription { get; set; }

    [Display(Name = "Описание")]
    public string? Description { get; set; }

    [Display(Name = "Цена")]
    public decimal Price { get; set; }

    [Display(Name = "Старая цена (для скидки)")]
    public decimal? OldPrice { get; set; }

    [Display(Name = "Фото")]
    public string? Photo { get; set; }

    [Display(Name = "Размер")]
    public string? Size { get; set; }         // "S,M,L,XL"

    [Display(Name = "Тип товара")]
    public ProductTypeEnum Type { get; set; } = ProductTypeEnum.Standard;

    [Display(Name = "Акция")]
    public bool IsPromo { get; set; }

    [Display(Name = "Новинка")]
    public bool IsNew { get; set; }

    [Display(Name = "В наличии")]
    public bool InStock { get; set; } = true;

    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}