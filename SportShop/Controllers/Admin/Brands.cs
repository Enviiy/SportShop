using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportShop.Domain.Entities;

namespace SportShop.Controllers.Admin;

[Authorize(Roles = "Admin")]
public partial class AdminController
{
    public async Task<IActionResult> Brands()
        => View(await _dataManager.Brands.GetAllAsync());

    public async Task<IActionResult> BrandsEdit(int id = 0)
    {
        Brand entity = id == 0
            ? new Brand()
            : await _dataManager.Brands.GetByIdAsync(id) ?? new Brand();
        return View(entity);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BrandsEdit(Brand entity, IFormFile? logoFile)
    {
        if (!ModelState.IsValid) return View(entity);

        if (logoFile != null && logoFile.Length > 0)
        {
            entity.Logo = logoFile.FileName;
            await SaveImg(logoFile);
        }

        await _dataManager.Brands.SaveAsync(entity);
        return RedirectToAction(nameof(Brands));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BrandsDelete(int id)
    {
        await _dataManager.Brands.DeleteAsync(id);
        return RedirectToAction(nameof(Brands));
    }
}