using ExampleEFCoreMigrate.Tests.CompatibilityTest.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExampleEFCoreMigrate.Tests.CompatibilityTest.Contexts;

/// <summary>
/// NotNullTodoItem に対応する DbContext
/// </summary>
public class NotNullApplicationDbContext : DbContext
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="options">DbContextのオプション</param>
    public NotNullApplicationDbContext(DbContextOptions<NotNullApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// TODOアイテムのDbSet
    /// </summary>
    public DbSet<NotNullTodoItem> TodoItems { get; set; }
}