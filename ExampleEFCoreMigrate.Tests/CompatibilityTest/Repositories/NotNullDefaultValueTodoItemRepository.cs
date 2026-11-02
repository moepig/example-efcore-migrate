using ExampleEFCoreMigrate.Tests.CompatibilityTest.Contexts;
using ExampleEFCoreMigrate.Tests.CompatibilityTest.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExampleEFCoreMigrate.Tests.CompatibilityTest.Repositories;

/// <summary>
/// TodoItemのデータ操作を実装するリポジトリ
/// </summary>
public class NotNullDefaultValueTodoItemRepository
{
    private readonly NotNullDefaultValueApplicationDbContext _context;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="context">データベースコンテキスト</param>
    public NotNullDefaultValueTodoItemRepository(NotNullDefaultValueApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<NotNullTodoItem?> GetByIdAsync(long id)
    {
        return await _context.TodoItems.FindAsync(id);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<NotNullTodoItem>> GetAllAsync()
    {
        return await _context.TodoItems.OrderBy(t => t.Timestamp).ToListAsync();
    }

    /// <inheritdoc />
    public async Task AddAsync(NotNullTodoItem todoItem)
    {
        await _context.TodoItems.AddAsync(todoItem);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task UpdateAsync(NotNullTodoItem todoItem)
    {
        // タイムスタンプを更新
        todoItem.Timestamp = DateTime.UtcNow;
        _context.TodoItems.Update(todoItem);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task DeleteAsync(long id)
    {
        var todoItem = await _context.TodoItems.FindAsync(id);
        if (todoItem != null)
        {
            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();
        }
    }
}
