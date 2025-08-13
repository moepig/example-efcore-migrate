using ExampleEFCoreMigrate.Models.Contexts;
using ExampleEFCoreMigrate.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExampleEFCoreMigrate.Repositories;

/// <summary>
/// TodoItemのデータ操作を実装するリポジトリ
/// </summary>
public class TodoItemRepository : ITodoItemRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="context">データベースコンテキスト</param>
    public TodoItemRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<TodoItem?> GetByIdAsync(long id)
    {
        return await _context.TodoItems.FindAsync(id);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TodoItem>> GetAllAsync()
    {
        return await _context.TodoItems.OrderBy(t => t.Timestamp).ToListAsync();
    }

    /// <inheritdoc />
    public async Task AddAsync(TodoItem todoItem)
    {
        await _context.TodoItems.AddAsync(todoItem);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task UpdateAsync(TodoItem todoItem)
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
