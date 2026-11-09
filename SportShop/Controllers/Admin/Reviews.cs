using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SportShop.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public partial class AdminController
    {
        public async Task<IActionResult> Reviews() => View(await _dataManager.Reviews.GetAllAsync());

        [HttpPost]
        public async Task<IActionResult> ApproveReview(int id)
        {
            await _dataManager.Reviews.ApproveAsync(id);
            return RedirectToAction("Reviews");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteReview(int id)
        {
            await _dataManager.Reviews.DeleteAsync(id);
            return RedirectToAction("Reviews");
        }
    }
}
