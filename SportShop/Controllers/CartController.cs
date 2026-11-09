using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportShop.Domain.Repositories;
using SportShop.Models;

namespace SportShop.Controllers;

[Authorize]
public class CartController : Controller
{
    private readonly DataManager _dm;
    private readonly UserManager<IdentityUser> _um;

    public CartController(DataManager dm, UserManager<IdentityUser> um)
    { _dm = dm; _um = um; }

    public async Task<IActionResult> Index()
    {
        var userId = _um.GetUserId(User)!;
        var items = await _dm.Cart.GetUserItemsAsync(userId);
        return View(new CartViewModel { Items = items });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(int productId, int quantity = 1, string? returnUrl = null)
    {
        var userId = _um.GetUserId(User)!;
        await _dm.Cart.AddAsync(userId, productId, quantity);
        if (!string.IsNullOrEmpty(returnUrl)) return Redirect(returnUrl);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, int quantity)
    {
        await _dm.Cart.UpdateQuantityAsync(id, quantity);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(int id)
    {
        await _dm.Cart.RemoveAsync(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Checkout()
    {
        var userId = _um.GetUserId(User)!;
        var items = await _dm.Cart.GetUserItemsAsync(userId);
        if (!items.Any()) return RedirectToAction(nameof(Index));
        return View(new CheckoutViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout(CheckoutViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var userId = _um.GetUserId(User)!;
        var order = await _dm.Orders.CreateFromCartAsync(userId, model.Name!, model.Phone!, model.Address!);
        return RedirectToAction(nameof(Success), new { id = order.Id });
    }

    public IActionResult Success(int id)
    {
        ViewBag.OrderId = id;
        return View();
    }
}