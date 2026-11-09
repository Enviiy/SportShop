using SportShop.Domain.Entities;
using SportShop.Domain.Enums;

namespace SportShop.Domain.Repositories.Abstract;

public interface IOrdersRepository
{
    Task<IEnumerable<Order>> GetAllAsync();
    Task<IEnumerable<Order>> GetByUserAsync(string userId);
    Task<Order?> GetByIdAsync(int id);
    Task<Order> CreateFromCartAsync(string userId, string name, string phone, string address);
    Task UpdateStatusAsync(int id, OrderStatusEnum status);
    Task<bool> HasUserBoughtProductAsync(string userId, int productId);
}