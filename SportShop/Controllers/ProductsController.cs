using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportShop.Domain.Entities;
using SportShop.Domain.Repositories;
using SportShop.Infrastructure;

namespace SportShop.Controllers;

public class ProductsController : Controller
{
    private readonly DataManager _dataManager;
    public ProductsController(DataManager dm) => _dataManager = dm;

    public async Task<IActionResult> Index(
        int? categoryId, int? brandId,
        decimal? priceMin, decimal? priceMax,
        string? search, string? size, string? sort)
    {
        IQueryable<Product> query = _dataManager.Products.Query()
                                                       .Include(p => p.Category)
                                                       .Include(p => p.Brand);
        if (categoryId.HasValue) query = query.Where(p => p.CategoryId == categoryId);
        if (brandId.HasValue) query = query.Where(p => p.BrandId == brandId);
        if (priceMin.HasValue) query = query.Where(p => p.Price >= priceMin);
        if (priceMax.HasValue) query = query.Where(p => p.Price <= priceMax);
        if (!string.IsNullOrWhiteSpace(size))
            query = query.Where(p => p.Size != null && p.Size.Contains(size));
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(p => p.Title.Contains(search));

        query = sort switch
        {
            "price_asc" => query.OrderBy(p => p.Price),
            "price_desc" => query.OrderByDescending(p => p.Price),
            "new" => query.OrderByDescending(p => p.DateCreated),
            _ => query.OrderBy(p => p.Title)
        };

        ViewBag.Categories = await _dataManager.Categories.GetAllAsync();
        ViewBag.Brands = await _dataManager.Brands.GetAllAsync();
        ViewBag.CurrentCategory = categoryId;
        ViewBag.CurrentBrand = brandId;
        ViewBag.Search = search;
        ViewBag.Size = size;
        ViewBag.Sort = sort;

        return View(DtoHelper.ProductsToDto(await query.ToListAsync()));
    }

    public async Task<IActionResult> Promo()
    {
        var items = await _dataManager.Products.Query()
                                               .Include(p => p.Category)
                                               .Include(p => p.Brand)
                                               .Where(p => p.IsPromo)
                                               .ToListAsync();
        return View("List", DtoHelper.ProductsToDto(items));
    }

    public async Task<IActionResult> New()
    {
        var items = await _dataManager.Products.Query()
                                               .Include(p => p.Category)
                                               .Include(p => p.Brand)
                                               .Where(p => p.IsNew)
                                               .OrderByDescending(p => p.DateCreated)
                                               .ToListAsync();
        return View("List", DtoHelper.ProductsToDto(items));
    }

    public async Task<IActionResult> Show(int id)
    {
        var p = await _dataManager.Products.GetProductByIdAsync(id);
        if (p is null) return NotFound();
        ViewBag.Reviews = p.Reviews.Where(r => r.IsApproved)
                                    .OrderByDescending(r => r.DateCreated)
                                    .ToList();
        return View(DtoHelper.ProductToDto(p));
    }
}