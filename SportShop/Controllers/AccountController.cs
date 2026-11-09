using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportShop.Domain.Repositories;
using SportShop.Models;

namespace SportShop.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly DataManager _dataManager;

    public AccountController(
        SignInManager<IdentityUser> signInManager,
        UserManager<IdentityUser> userManager,
        DataManager dataManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _dataManager = dataManager;
    }

    // ---------- ВХОД ----------
    [HttpGet]
    public async Task<IActionResult> Login(string? returnUrl)
    {
        await _signInManager.SignOutAsync();
        ViewBag.ReturnUrl = returnUrl;
        return View(new LoginViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)
    {
        ViewBag.ReturnUrl = returnUrl ?? "";
        if (!ModelState.IsValid) return View(model);

        var result = await _signInManager.PasswordSignInAsync(
            model.UserName!, model.Password!, model.RememberMe, lockoutOnFailure: false);

        if (result.Succeeded) return Redirect(returnUrl ?? "/");

        ModelState.AddModelError(string.Empty, "Неверный логин или пароль.");
        return View(model);
    }

    // ---------- РЕГИСТРАЦИЯ ----------
    [HttpGet]
    public IActionResult Register() => View(new RegisterViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = new IdentityUser { UserName = model.Email, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password!);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "User");
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        foreach (var err in result.Errors)
            ModelState.AddModelError(string.Empty, err.Description);

        return View(model);
    }

    // ---------- ВЫХОД ----------
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    // ---------- ЛИЧНЫЙ КАБИНЕТ ----------
    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is null) return RedirectToAction(nameof(Login));
        return View(user);
    }

    [Authorize]
    public async Task<IActionResult> Orders()
    {
        var userId = _userManager.GetUserId(User)!;
        var orders = await _dataManager.Orders.GetByUserAsync(userId);
        return View(orders);
    }

    public IActionResult AccessDenied() => View();
}