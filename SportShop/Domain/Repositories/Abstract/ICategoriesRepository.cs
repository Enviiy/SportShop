using SportShop.Domain.Entities;

namespace SportShop.Domain.Repositories.Abstract;

public interface ICategoriesRepository
{
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int id);
    Task SaveAsync(Category entity);
    Task DeleteAsync(int id);
}