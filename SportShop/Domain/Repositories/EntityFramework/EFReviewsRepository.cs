using Microsoft.EntityFrameworkCore;
using SportShop.Domain.Entities;
using SportShop.Domain.Repositories.Abstract;

namespace SportShop.Domain.Repositories.EntityFramework;

public class EFReviewsRepository : IReviewsRepository
{
    private readonly AppDbContext _ctx;
    public EFReviewsRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<Review>> GetAllAsync() =>
        await _ctx.Reviews.AsNoTracking()
                          .Include(r => r.Product)
                          .OrderByDescending(r => r.DateCreated)
                          .ToListAsync();

    public async Task<IEnumerable<Review>> GetApprovedAsync() =>
        await _ctx.Reviews.AsNoTracking()
                          .Include(r => r.Product)
                          .Where(r => r.IsApproved)
                          .OrderByDescending(r => r.DateCreated)
                          .ToListAsync();

    public async Task<IEnumerable<Review>> GetByProductAsync(int productId) =>
        await _ctx.Reviews.AsNoTracking()
                          .Where(r => r.ProductId == productId && r.IsApproved)
                          .OrderByDescending(r => r.DateCreated)
                          .ToListAsync();

    public async Task<Review?> GetByIdAsync(int id) =>
        await _ctx.Reviews.FirstOrDefaultAsync(r => r.Id == id);

    public async Task AddAsync(Review entity)
    {
        _ctx.Reviews.Add(entity);
        await _ctx.SaveChangesAsync();
    }

    public async Task ApproveAsync(int id)
    {
        var r = await _ctx.Reviews.FindAsync(id);
        if (r != null) { r.IsApproved = true; await _ctx.SaveChangesAsync(); }
    }

    public async Task DeleteAsync(int id)
    {
        var r = await _ctx.Reviews.FindAsync(id);
        if (r != null) { _ctx.Reviews.Remove(r); await _ctx.SaveChangesAsync(); }
    }
}