using SportShop.Domain.Entities;
using SportShop.Models;

public static class DtoHelper
{
    public static ProductDto ProductToDto(Product e) => new()
    {
        Id = e.Id,
        Title = e.Title,
        ShortDescription = e.ShortDescription,
        Description = e.Description,
        Photo = e.Photo,
        CategoryName = e.Category?.Title,
        BrandName = e.Brand?.Title,
        Price = e.Price,
        OldPrice = e.OldPrice,
        Size = e.Size,
        Type = e.Type.ToString(),
        IsPromo = e.IsPromo,
        IsNew = e.IsNew
    };

    public static IEnumerable<ProductDto> ProductsToDto(IEnumerable<Product> list)
        => list.Select(ProductToDto).ToList();
}