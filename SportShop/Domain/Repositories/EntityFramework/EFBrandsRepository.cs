using Microsoft.EntityFrameworkCore;
using SportShop.Domain.Entities;
using SportShop.Domain.Repositories.Abstract;

namespace SportShop.Domain.Repositories.EntityFramework;

public class EFBrandsRepository : IBrandsRepository
{
    private readonly AppDbContext _ctx;
    public EFBrandsRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<Brand>> GetAllAsync() =>
        await _ctx.Brands.AsNoTracking().OrderBy(b => b.Title).ToListAsync();

    public async Task<Brand?> GetByIdAsync(int id) =>
        await _ctx.Brands.Include(b => b.Products).FirstOrDefaultAsync(b => b.Id == id);

    public async Task SaveAsync(Brand entity)
    {
        _ctx.Entry(entity).State = entity.Id == default ? EntityState.Added : EntityState.Modified;
        await _ctx.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        _ctx.Entry(new Brand { Id = id }).State = EntityState.Deleted;
        await _ctx.SaveChangesAsync();
    }
}