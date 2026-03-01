using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepositor;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Bulky.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using Stripe;
using Bulky.DataAccess.DbInitializer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options =>
{
    // 這裡是設定 Identity 的 Cookie 路徑，當使用者需要登入、登出或是存取被拒絕的頁面時，會導向這些路徑
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});
builder.Services.AddAuthentication().AddFacebook(options =>
{
    options.AppId = "843234465425120";
    options.AppSecret = "d594e8aee52489bd1bcadd4079f09aed";
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100); // 設定 Session 的過期時間
    options.Cookie.HttpOnly = true; // 設定 Cookie 為 HttpOnly，增加安全性
    options.Cookie.IsEssential = true; // 設定 Cookie 為必要，確保在使用者同意前也能使用 Session
});

builder.Services.AddScoped<IDbInitializer, DbInitializer>();
// identity defalt uses razor page
builder.Services.AddRazorPages();
// 這裡就是註冊 Repository 的地方
// AddScoped 代表在同一個 HTTP Request 中會使用同一個實例
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailSender, EmailSender>(); // 註冊 EmailSender 服務
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();
// 這裡是設定 Stripe 的 API Key，從 appsettings.json 中讀取 SecretKey 的值
app.UseRouting();
app.UseAuthentication();
//after user authentication, we need to authorize
app.UseAuthorization(); 
app.UseSession(); // 這裡是啟用 Session 的中介軟體，必須在 UseRouting 和 UseEndpoints 之間呼叫
SeedDatabase(); // 這裡是呼叫 SeedDatabase 方法來初始化資料庫，確保在應用程式啟動時就有必要的資料
app.MapRazorPages(); // 這裡是用來對應 Razor Page 的路由
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();

void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();
    }
}
