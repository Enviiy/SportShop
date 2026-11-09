using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportShop.Domain.Entities;

namespace SportShop.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public partial class AdminController
    {
        public async Task<IActionResult> Products()
            => View(await _dataManager.Products.GetProductsAsync());

        public async Task<IActionResult> ProductsEdit(int id = 0)
        {
            Product entity = id == default
                ? new Product()
                : await _dataManager.Products.GetProductByIdAsync(id) ?? new Product();

            ViewBag.Categories = await _dataManager.Categories.GetAllAsync();
            ViewBag.Brands = await _dataManager.Brands.GetAllAsync();
            return View(entity);
        }

        [HttpPost]
        public async Task<IActionResult> ProductsEdit(Product entity, IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _dataManager.Categories.GetAllAsync();
                ViewBag.Brands = await _dataManager.Brands.GetAllAsync();
                return View(entity);
            }

            if (imageFile != null)
            {
                entity.Photo = imageFile.FileName;
                await SaveImg(imageFile);
            }

            await _dataManager.Products.SaveProductAsync(entity);
            return RedirectToAction("Products");
        }

        [HttpPost]
        public async Task<IActionResult> ProductsDelete(int id)
        {
            await _dataManager.Products.DeleteProductAsync(id);
            return RedirectToAction("Products");
        }
    }
}
