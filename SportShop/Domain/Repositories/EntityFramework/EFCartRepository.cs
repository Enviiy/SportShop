using Microsoft.EntityFrameworkCore;
using SportShop.Domain.Entities;
using SportShop.Domain.Repositories.Abstract;

namespace SportShop.Domain.Repositories.EntityFramework;

public class EFCartRepository : ICartRepository
{
    private readonly AppDbContext _ctx;
    public EFCartRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<CartItem>> GetUserItemsAsync(string userId) =>
        await _ctx.CartItems.Include(c => c.Product).ThenInclude(p => p!.Category)
                            .Where(c => c.UserId == userId)
                            .ToListAsync();

    public async Task AddAsync(string userId, int productId, int quantity)
    {
        var existing = await _ctx.CartItems
            .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);
        if (existing != null)
        {
            existing.Quantity += quantity;
        }
        else
        {
            _ctx.CartItems.Add(new CartItem
            {
                UserId = userId,
                ProductId = productId,
                Quantity = quantity
            });
        }
        await _ctx.SaveChangesAsync();
    }

    public async Task UpdateQuantityAsync(int id, int quantity)
    {
        var item = await _ctx.CartItems.FindAsync(id);
        if (item == null) return;
        if (quantity <= 0) _ctx.CartItems.Remove(item);
        else item.Quantity = quantity;
        await _ctx.SaveChangesAsync();
    }

    public async Task RemoveAsync(int id)
    {
        var item = await _ctx.CartItems.FindAsync(id);
        if (item != null) { _ctx.CartItems.Remove(item); await _ctx.SaveChangesAsync(); }
    }

    public async Task ClearAsync(string userId)
    {
        var items = await _ctx.CartItems.Where(c => c.UserId == userId).ToListAsync();
        _ctx.CartItems.RemoveRange(items);
        await _ctx.SaveChangesAsync();
    }

    public async Task<int> GetCountAsync(string userId) =>
        await _ctx.CartItems.Where(c => c.UserId == userId).SumAsync(c => (int?)c.Quantity) ?? 0;
}