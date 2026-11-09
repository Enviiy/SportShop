using Microsoft.EntityFrameworkCore;
using SportShop.Domain.Entities;
using SportShop.Domain.Repositories.Abstract;

namespace SportShop.Domain.Repositories.EntityFramework;

public class EFCategoriesRepository : ICategoriesRepository
{
    private readonly AppDbContext _ctx;
    public EFCategoriesRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<Category>> GetAllAsync() =>
        await _ctx.Categories.AsNoTracking().OrderBy(c => c.Title).ToListAsync();

    public async Task<Category?> GetByIdAsync(int id) =>
        await _ctx.Categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.Id == id);

    public async Task SaveAsync(Category entity)
    {
        _ctx.Entry(entity).State = entity.Id == default ? EntityState.Added : EntityState.Modified;
        await _ctx.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        _ctx.Entry(new Category { Id = id }).State = EntityState.Deleted;
        await _ctx.SaveChangesAsync();
    }
}