using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportShop.Domain.Repositories;

namespace SportShop.Controllers.Admin;

[Authorize(Roles = "Admin,Manager")]
public partial class AdminController : Controller
{
    private readonly DataManager _dataManager;
    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly ILogger<AdminController> _logger;

    // Добавляем менеджеры пользователей и ролей
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminController(
        DataManager dataManager,
        IWebHostEnvironment hostEnvironment,
        ILogger<AdminController> logger,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _dataManager = dataManager;
        _hostEnvironment = hostEnvironment;
        _logger = logger;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.ProductsCount = (await _dataManager.Products.GetProductsAsync()).Count();
        ViewBag.OrdersCount = (await _dataManager.Orders.GetAllAsync()).Count();
        ViewBag.ReviewsCount = (await _dataManager.Reviews.GetAllAsync()).Count(r => !r.IsApproved);
        return View();
    }

    private async Task<string> SaveImg(IFormFile file)
    {
        string imgFolder = Path.Combine(_hostEnvironment.WebRootPath, "img");
        if (!Directory.Exists(imgFolder)) Directory.CreateDirectory(imgFolder);

        string path = Path.Combine(imgFolder, file.FileName);
        await using FileStream stream = new(path, FileMode.Create);
        await file.CopyToAsync(stream);

        _logger.LogInformation("Сохранено изображение: {Path}", path);
        return file.FileName;
    }
}