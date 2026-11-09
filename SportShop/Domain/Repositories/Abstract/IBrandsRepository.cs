using SportShop.Domain.Entities;

namespace SportShop.Domain.Repositories.Abstract;

public interface IBrandsRepository
{
    Task<IEnumerable<Brand>> GetAllAsync();
    Task<Brand?> GetByIdAsync(int id);
    Task SaveAsync(Brand entity);
    Task DeleteAsync(int id);
}