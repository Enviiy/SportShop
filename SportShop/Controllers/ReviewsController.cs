using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportShop.Domain.Entities;
using SportShop.Domain.Repositories;
using SportShop.Models;

namespace SportShop.Controllers;

public class ReviewsController : Controller
{
    private readonly DataManager _dm;
    private readonly UserManager<IdentityUser> _um;
    public ReviewsController(DataManager dm, UserManager<IdentityUser> um)
    {
        _dm = dm; _um = um;
    }

    public async Task<IActionResult> Index()
        => View(await _dm.Reviews.GetApprovedAsync());

    [Authorize]
    [HttpGet]
    public IActionResult Create(int productId)
        => View(new ReviewViewModel { ProductId = productId });

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ReviewViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var userId = _um.GetUserId(User)!;
        if (!await _dm.Orders.HasUserBoughtProductAsync(userId, model.ProductId))
        {
            ModelState.AddModelError(string.Empty,
                "Оставлять отзывы можно только после покупки этого товара.");
            return View(model);
        }

        await _dm.Reviews.AddAsync(new Review
        {
            UserId = userId,
            AuthorName = User.Identity?.Name,
            ProductId = model.ProductId,
            Text = model.Text,
            Rating = model.Rating,
            IsApproved = false
        });
        TempData["Success"] = "Спасибо! Отзыв отправлен на модерацию.";
        return RedirectToAction("Index");
    }
}