using Microsoft.EntityFrameworkCore;
using SportShop.Domain.Entities;
using SportShop.Domain.Enums;
using SportShop.Domain.Repositories.Abstract;

namespace SportShop.Domain.Repositories.EntityFramework;

public class EFOrdersRepository : IOrdersRepository
{
    private readonly AppDbContext _ctx;
    public EFOrdersRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<Order>> GetAllAsync() =>
        await _ctx.Orders.AsNoTracking()
                         .Include(o => o.Items).ThenInclude(i => i.Product)
                         .OrderByDescending(o => o.DateCreated)
                         .ToListAsync();

    public async Task<IEnumerable<Order>> GetByUserAsync(string userId) =>
        await _ctx.Orders.AsNoTracking()
                         .Include(o => o.Items).ThenInclude(i => i.Product)
                         .Where(o => o.UserId == userId)
                         .OrderByDescending(o => o.DateCreated)
                         .ToListAsync();

    public async Task<Order?> GetByIdAsync(int id) =>
        await _ctx.Orders.Include(o => o.Items).ThenInclude(i => i.Product)
                         .FirstOrDefaultAsync(o => o.Id == id);

    public async Task<Order> CreateFromCartAsync(string userId, string name, string phone, string address)
    {
        var cartItems = await _ctx.CartItems.Include(c => c.Product)
                                            .Where(c => c.UserId == userId)
                                            .ToListAsync();
        if (!cartItems.Any()) throw new InvalidOperationException("Корзина пуста.");

        var order = new Order
        {
            UserId = userId,
            Title = $"Заказ от {DateTime.UtcNow:dd.MM.yyyy HH:mm}",
            CustomerName = name,
            Phone = phone,
            Address = address,
            Total = cartItems.Sum(i => i.Quantity * (i.Product?.Price ?? 0)),
            Status = OrderStatusEnum.New,
            Items = cartItems.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = i.Product?.Price ?? 0
            }).ToList()
        };
        _ctx.Orders.Add(order);
        _ctx.CartItems.RemoveRange(cartItems);
        await _ctx.SaveChangesAsync();
        return order;
    }

    public async Task UpdateStatusAsync(int id, OrderStatusEnum status)
    {
        var order = await _ctx.Orders.FindAsync(id);
        if (order != null) { order.Status = status; await _ctx.SaveChangesAsync(); }
    }

    public async Task<bool> HasUserBoughtProductAsync(string userId, int productId) =>
        await _ctx.Orders.AsNoTracking()
                         .Where(o => o.UserId == userId && o.Status != OrderStatusEnum.Cancelled)
                         .SelectMany(o => o.Items)
                         .AnyAsync(i => i.ProductId == productId);
}