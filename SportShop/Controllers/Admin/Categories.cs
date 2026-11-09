using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportShop.Domain.Entities;

namespace SportShop.Controllers.Admin;

[Authorize(Roles = "Admin")]
public partial class AdminController
{
    public async Task<IActionResult> Categories()
        => View(await _dataManager.Categories.GetAllAsync());

    public async Task<IActionResult> CategoriesEdit(int id = 0)
    {
        Category entity = id == 0
            ? new Category()
            : await _dataManager.Categories.GetByIdAsync(id) ?? new Category();
        return View(entity);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CategoriesEdit(Category entity)
    {
        if (!ModelState.IsValid) return View(entity);

        await _dataManager.Categories.SaveAsync(entity);
        _logger.LogInformation("Добавлена/обновлена категория ID: {Id}", entity.Id);

        return RedirectToAction(nameof(Categories));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CategoriesDelete(int id)
    {
        try
        {
            await _dataManager.Categories.DeleteAsync(id);
            _logger.LogInformation("Удалена категория ID: {Id}", id);
        }
        catch (DbUpdateException)
        {
            TempData["Error"] = "Нельзя удалить категорию: на неё ссылаются товары.";
            _logger.LogWarning("Попытка удалить категорию ID={Id}, на которую есть ссылки", id);
        }
        return RedirectToAction(nameof(Categories));
    }
}