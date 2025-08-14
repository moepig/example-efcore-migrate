using Microsoft.EntityFrameworkCore;

namespace ExampleEFCoreMigrate.Tests.CompatibilityTest.Contexts;

/// <summary>
/// NullableTodoItem に対応する DbContext
/// </summary>
public class NullableApplicationDbContext : DbContext
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="options">DbContextのオプション</param>
    public NullableApplicationDbContext(DbContextOptions<NullableApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// TODOアイテムのDbSet
    /// </summary>
    public DbSet<NullableTodoItem> TodoItems { get; set; }
}