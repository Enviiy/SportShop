using Microsoft.AspNetCore.Mvc;
using SportShop.Domain.Repositories;
using SportShop.Infrastructure;

namespace SportShop.Controllers;

public class BrandsController : Controller
{
    private readonly DataManager _dataManager;
    public BrandsController(DataManager dm) => _dataManager = dm;

    public async Task<IActionResult> Index()
    {
        var brands = await _dataManager.Brands.GetAllAsync();
        return View(brands);
    }

    public async Task<IActionResult> Show(int id)
    {
        var brand = await _dataManager.Brands.GetByIdAsync(id);
        if (brand is null) return NotFound();

        ViewBag.Products = DtoHelper.ProductsToDto(brand.Products);
        return View(brand);
    }
}