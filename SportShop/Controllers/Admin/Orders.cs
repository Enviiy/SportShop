using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportShop.Domain.Enums;

namespace SportShop.Controllers.Admin
{
    [Authorize(Roles = "Admin,Manager")]
    public partial class AdminController
    {
        public async Task<IActionResult> Orders() => View(await _dataManager.Orders.GetAllAsync());

        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(int id, OrderStatusEnum status)
        {
            await _dataManager.Orders.UpdateStatusAsync(id, status);
            return RedirectToAction("Orders");
        }
    }
}
