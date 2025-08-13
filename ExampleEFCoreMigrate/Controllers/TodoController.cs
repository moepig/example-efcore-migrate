using ExampleEFCoreMigrate.Models.Entities;
using ExampleEFCoreMigrate.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ExampleEFCoreMigrate.Controllers;

/// <summary>
/// TODOリストの表示・登録・削除を行うコントローラー
/// </summary>
public class TodoController : Controller
{

    private readonly ILogger<TodoController> _logger;
    private readonly ITodoItemRepository _repository;

    public TodoController(ILogger<TodoController> logger, ITodoItemRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    /// <summary>
    /// TODOリスト表示画面
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var items = await _repository.GetAllAsync();

        return View(items);
    }

    /// <summary>
    /// TODO新規登録
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            ModelState.AddModelError("title", "タイトルは必須です。");
            var items = await _repository.GetAllAsync();
            return View("Index", items);
        }
        var todo = new TodoItem { Title = title, IsCompleted = false, Timestamp = DateTime.UtcNow };
        await _repository.AddAsync(todo);

        return RedirectToAction("Index");
    }

    /// <summary>
    /// TODO削除処理
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(long id)
    {
        await _repository.DeleteAsync(id);

        return View("DeleteComplete");
    }

    /// <summary>
    /// TODO完了状態の切り替え
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Complete(long id)
    {
        var todo = await _repository.GetByIdAsync(id);
        if (todo != null)
        {
            todo.IsCompleted = !todo.IsCompleted; // 完了状態をトグル
            await _repository.UpdateAsync(todo);
        }
        return RedirectToAction("Index");
    }
}
