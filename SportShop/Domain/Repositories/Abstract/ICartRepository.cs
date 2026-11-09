using SportShop.Domain.Entities;

namespace SportShop.Domain.Repositories.Abstract;

public interface ICartRepository
{
    Task<IEnumerable<CartItem>> GetUserItemsAsync(string userId);
    Task AddAsync(string userId, int productId, int quantity);
    Task UpdateQuantityAsync(int id, int quantity);
    Task RemoveAsync(int id);
    Task ClearAsync(string userId);
    Task<int> GetCountAsync(string userId);
}