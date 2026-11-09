using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SportShop.Domain.Entities;
using System.Reflection.Emit;

namespace SportShop.Domain;

public class AppDbContext : IdentityDbContext<IdentityUser>
{
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Brand> Brands { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Review> Reviews { get; set; } = null!;
    public DbSet<CartItem> CartItems { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderItem> OrderItems { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // --- Роли ---
        string adminRoleId = "DA36513F-6393-43B8-A1E1-5CDF3DBEB6BF";
        string managerRoleId = "B1111111-1111-1111-1111-111111111111";
        string userRoleId = "C2222222-2222-2222-2222-222222222222";

        builder.Entity<IdentityRole>().HasData(
            new IdentityRole { Id = adminRoleId, Name = "Admin", NormalizedName = "ADMIN" },
            new IdentityRole { Id = managerRoleId, Name = "Manager", NormalizedName = "MANAGER" },
            new IdentityRole { Id = userRoleId, Name = "User", NormalizedName = "USER" }
        );

        // --- Пользователь-админ ---
        string adminUserId = "FA2325ED-7786-447A-BA83-93C89FE17D93";
        var hasher = new PasswordHasher<IdentityUser>();
        var adminUser = new IdentityUser
        {
            Id = adminUserId,
            UserName = "admin",
            NormalizedUserName = "ADMIN",
            Email = "admin@sportshop.ru",
            NormalizedEmail = "ADMIN@SPORTSHOP.RU",
            EmailConfirmed = true,
            SecurityStamp = string.Empty
        };
        adminUser.PasswordHash = hasher.HashPassword(adminUser, "admin123");
        builder.Entity<IdentityUser>().HasData(adminUser);

        builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string> { RoleId = adminRoleId, UserId = adminUserId }
        );

        // каскадное удаление Product при удалении Category НЕ отключаем (упрощаем),
        // но в методичке показан приём OnDelete(DeleteBehavior.Restrict) — для продакшена лучше так:
        builder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}