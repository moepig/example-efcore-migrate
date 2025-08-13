using ExampleEFCoreMigrate.Repositories;
using ExampleEFCoreMigrate.Models.Contexts;
using Microsoft.EntityFrameworkCore; // DbContext拡張のため追加

var builder = WebApplication.CreateBuilder(args);

// appsettings.json から接続文字列を取得
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// サービス登録
builder.Services.AddControllersWithViews();
// DbContext登録（MySQL用接続文字列を利用）
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);
// リポジトリ登録
builder.Services.AddScoped<ITodoItemRepository, TodoItemRepository>();

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

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Todo}/{action=Index}/{id?}");

app.Run();
