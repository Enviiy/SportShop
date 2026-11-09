using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SportShop.Domain;
using SportShop.Domain.Repositories;
using SportShop.Domain.Repositories.Abstract;
using SportShop.Domain.Repositories.EntityFramework;
using SportShop.Infrastructure;

namespace SportShop;

public class Program
{
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(builder.Environment.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
        IConfiguration configuration = configurationBuilder.Build();
        AppConfig? appConfig = configuration.GetSection("Project").Get<AppConfig>();

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
        builder.Host.UseSerilog();

        // БД
        builder.Services.AddDbContext<AppDbContext>(x =>
            x.UseSqlServer(appConfig!.database.connectionString)
             .ConfigureWarnings(w => w.Ignore(
                 Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning)));

        // Identity
        builder.Services.AddIdentity<IdentityUser, IdentityRole>(opts =>
        {
            opts.User.RequireUniqueEmail = true;
            opts.Password.RequiredLength = 6;
            opts.Password.RequireNonAlphanumeric = false;
            opts.Password.RequireLowercase = false;
            opts.Password.RequireUppercase = false;
            opts.Password.RequireDigit = false;
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        builder.Services.ConfigureApplicationCookie(opts =>
        {
            opts.Cookie.Name = "sportshop.auth";
            opts.Cookie.HttpOnly = true;
            opts.LoginPath = "/Account/Login";
            opts.AccessDeniedPath = "/Account/AccessDenied";
            opts.SlidingExpiration = true;
        });

        // DI репозиториев
        builder.Services.AddTransient<IProductsRepository, EFProductsRepository>();
        builder.Services.AddTransient<ICategoriesRepository, EFCategoriesRepository>();
        builder.Services.AddTransient<IBrandsRepository, EFBrandsRepository>();
        builder.Services.AddTransient<IReviewsRepository, EFReviewsRepository>();
        builder.Services.AddTransient<IOrdersRepository, EFOrdersRepository>();
        builder.Services.AddTransient<ICartRepository, EFCartRepository>();
        builder.Services.AddTransient<DataManager>();

        builder.Services.AddControllersWithViews();

        WebApplication app = builder.Build();

        app.UseStaticFiles();
        app.UseRouting();
        app.UseCookiePolicy();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

        // Автомиграции при старте (для демонстрации)
        using (IServiceScope scope = app.Services.CreateScope())
        {
            AppDbContext db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await db.Database.MigrateAsync();
        }

        await app.RunAsync();
    }
}