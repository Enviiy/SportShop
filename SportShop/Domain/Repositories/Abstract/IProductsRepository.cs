using SportShop.Domain.Entities;

public interface IProductsRepository
{
    Task<IEnumerable<Product>> GetProductsAsync();
    Task<Product?> GetProductByIdAsync(int id);
    Task SaveProductAsync(Product entity);
    Task DeleteProductAsync(int id);
    IQueryable<Product> Query();
}