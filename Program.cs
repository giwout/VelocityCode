using CarMagazine2025_42.Data.Context;
using CarMagazine2025_42.Data.Models;
using CarMagazine2025_42.Data.Models.Cart;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Добавление сервисов
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BikeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<BikeDbContext>()
.AddDefaultTokenProviders();

// 2. Настройки сессий и кэширования
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// 3. Регистрация кастомных сервисов
builder.Services.AddScoped<ShopCart>(sp => ShopCart.GetCart(sp));
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// 4. Настройка конвейера обработки запросов (Middleware)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Инициализация базы данных и админа (замените ваш текущий блок в Program.cs на этот)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<BikeDbContext>();
    context.Database.Migrate();

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

    if (!await roleManager.RoleExistsAsync("Admin"))
        await roleManager.CreateAsync(new IdentityRole("Admin"));

    var adminEmail = "admin@velocity.ru";
    if (await userManager.FindByEmailAsync(adminEmail) == null)
    {
        var admin = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            FirstName = "Admin",
            LastName = "Administrator",
            RegistrationDate = DateTime.Now
        };

        var res = await userManager.CreateAsync(admin, "Admin123!");
        if (res.Succeeded) await userManager.AddToRoleAsync(admin, "Admin");
        // ВАЖНО: Никаких вызовов SignInAsync здесь быть не должно!
    }
}

app.Run();