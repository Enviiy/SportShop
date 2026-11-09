using Microsoft.EntityFrameworkCore;
using SportShop.Domain.Entities;
using SportShop.Domain.Repositories.Abstract;

namespace SportShop.Domain.Repositories.EntityFramework;

public class EFProductsRepository : IProductsRepository
{
    private readonly AppDbContext _ctx;
    public EFProductsRepository(AppDbContext ctx) => _ctx = ctx;

    public IQueryable<Product> Query() => _ctx.Products.AsQueryable();

    public async Task<IEnumerable<Product>> GetProductsAsync() =>
        await _ctx.Products
                  .Include(p => p.Category)
                  .Include(p => p.Brand)
                  .ToListAsync();

    public async Task<Product?> GetProductByIdAsync(int id) =>
        await _ctx.Products
                  .Include(p => p.Category)
                  .Include(p => p.Brand)
                  .Include(p => p.Reviews)
                  .FirstOrDefaultAsync(p => p.Id == id);

    public async Task SaveProductAsync(Product entity)
    {
        _ctx.Entry(entity).State = entity.Id == default ? EntityState.Added : EntityState.Modified;
        await _ctx.SaveChangesAsync();
    }

    public async Task DeleteProductAsync(int id)
    {
        _ctx.Entry(new Product { Id = id }).State = EntityState.Deleted;
        await _ctx.SaveChangesAsync();
    }
}