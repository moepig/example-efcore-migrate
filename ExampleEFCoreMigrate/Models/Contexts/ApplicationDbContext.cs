using ExampleEFCoreMigrate.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExampleEFCoreMigrate.Models.Contexts;

/// <summary>
/// アプリケーションのデータベースコンテキスト
/// </summary>
public class ApplicationDbContext : DbContext
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="options">DbContextのオプション</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// TODOアイテムのDbSet
    /// </summary>
    public DbSet<TodoItem> TodoItems { get; set; }
}