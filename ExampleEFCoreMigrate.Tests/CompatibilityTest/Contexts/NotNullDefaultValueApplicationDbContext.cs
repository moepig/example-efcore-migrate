using ExampleEFCoreMigrate.Tests.CompatibilityTest.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExampleEFCoreMigrate.Tests.CompatibilityTest.Contexts;

/// <summary>
/// NotNullTodoItem に対応する DbContext
/// NOT NULL なカラムに対してデフォルト値を指定する
/// (アノテーションで指定することは出来ないので、DbContext を分けてバリエーションを実現している)
/// </summary>
public class NotNullDefaultValueApplicationDbContext : DbContext
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="options">DbContextのオプション</param>
    public NotNullDefaultValueApplicationDbContext(DbContextOptions<NotNullDefaultValueApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// TODOアイテムのDbSet
    /// </summary>
    public DbSet<NotNullTodoItem> TodoItems { get; set; }

    /// <summary>
    /// モデルの構成
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // NewColumn にデフォルト値 100 を設定
        modelBuilder.Entity<NotNullTodoItem>()
            .Property(e => e.NewColumn)
            .HasDefaultValue(100);
    }
}