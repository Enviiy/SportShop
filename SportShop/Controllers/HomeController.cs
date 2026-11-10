using Microsoft.AspNetCore.Mvc;
using SportShop.Domain.Repositories;
using SportShop.Infrastructure;
using SportShop.Models;

namespace SportShop.Controllers;

public class HomeController : Controller
{
    private readonly DataManager _dataManager;
    private readonly ILogger<HomeController> _logger;

    public HomeController(DataManager dataManager, ILogger<HomeController> logger)
    {
        _dataManager = dataManager;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var all = await _dataManager.Products.GetProductsAsync();
        ViewBag.AllProducts = DtoHelper.ProductsToDto(all);
        ViewBag.Popular = DtoHelper.ProductsToDto(all.Take(8));
        ViewBag.Promo = DtoHelper.ProductsToDto(all.Where(p => p.IsPromo).Take(4));
        ViewBag.Reviews = (await _dataManager.Reviews.GetApprovedAsync()).Take(3);
        return View();
    }

    public IActionResult About() => View();

    public IActionResult Contacts() => View();

    public IActionResult Team() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Feedback(string name, string email, string message)
    {
        // здесь можно отправить письмо или сохранить в БД
        _logger.LogInformation("Обратная связь от {Name} ({Email}): {Message}",
            name, email, message);
        ViewBag.Name = name;
        return View();
    }
}