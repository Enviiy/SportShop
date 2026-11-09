using SportShop.Domain.Entities;

namespace SportShop.Domain.Repositories.Abstract;

public interface IReviewsRepository
{
    Task<IEnumerable<Review>> GetAllAsync();
    Task<IEnumerable<Review>> GetApprovedAsync();
    Task<IEnumerable<Review>> GetByProductAsync(int productId);
    Task<Review?> GetByIdAsync(int id);
    Task AddAsync(Review entity);
    Task ApproveAsync(int id);
    Task DeleteAsync(int id);
}