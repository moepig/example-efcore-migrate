using ExampleEFCoreMigrate.Models.Entities;

namespace ExampleEFCoreMigrate.Repositories;

/// <summary>
/// TodoItemのデータ操作を定義するリポジトリインターフェース
/// </summary>
public interface ITodoItemRepository
{
    /// <summary>
    /// 指定されたIDのTodoItemを取得します。
    /// </summary>
    /// <param name="id">取得するTodoItemのID。</param>
    /// <returns>見つかったTodoItem。見つからない場合はnull。</returns>
    Task<TodoItem?> GetByIdAsync(long id);

    /// <summary>
    /// すべてのTodoItemを取得します。
    /// </summary>
    /// <returns>すべてのTodoItemのコレクション。</returns>
    Task<IEnumerable<TodoItem>> GetAllAsync();

    /// <summary>
    /// 新しいTodoItemを追加します。
    /// </summary>
    /// <param name="todoItem">追加するTodoItem。</param>
    Task AddAsync(TodoItem todoItem);

    /// <summary>
    /// 既存のTodoItemを更新します。
    /// </summary>
    /// <param name="todoItem">更新するTodoItem。</param>
    Task UpdateAsync(TodoItem todoItem);

    /// <summary>
    /// 指定されたIDのTodoItemを削除します。
    /// </summary>
    /// <param name="id">削除するTodoItemのID。</param>
    Task DeleteAsync(long id);
}