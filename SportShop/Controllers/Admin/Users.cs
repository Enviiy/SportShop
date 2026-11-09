using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SportShop.Controllers.Admin;

public partial class AdminController
{
    // Только Администратор может управлять пользователями (Менеджеру запрещено)
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Users()
    {
        var users = await _userManager.Users.ToListAsync();
        var userRoles = new Dictionary<string, IList<string>>();

        // Получаем роли для каждого пользователя
        foreach (var user in users)
        {
            userRoles[user.Id] = await _userManager.GetRolesAsync(user);
        }

        ViewBag.UserRoles = userRoles;
        // Список всех существующих ролей (Admin, Manager, User)
        ViewBag.AllRoles = _roleManager.Roles.Select(r => r.Name).ToList();

        return View(users);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateUserRole(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound();

        // Удаляем текущие роли и назначаем новую
        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        await _userManager.AddToRoleAsync(user, roleName);

        _logger.LogInformation("Пользователю {Email} назначена роль {Role}", user.Email, roleName);
        return RedirectToAction(nameof(Users));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound();

        // Защита от удаления самого себя
        var currentUserId = _userManager.GetUserId(User);
        if (user.Id == currentUserId)
        {
            TempData["Error"] = "Вы не можете удалить собственный аккаунт.";
            return RedirectToAction(nameof(Users));
        }

        await _userManager.DeleteAsync(user);
        _logger.LogWarning("Удален пользователь {Email}", user.Email);
        return RedirectToAction(nameof(Users));
    }
}