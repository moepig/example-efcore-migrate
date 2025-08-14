using ExampleEFCoreMigrate.Tests.CompatibilityTest.Contexts;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MySql;

namespace ExampleEFCoreMigrate.Tests.CompatibilityTest;

[TestFixture]
public class NewerDatabase_Nullable_TodoItemRepositoryTest
{
    private MySqlContainer _mysqlContainer;
    private ExampleEFCoreMigrate.Models.Contexts.ApplicationDbContext _dbContext;
    private ExampleEFCoreMigrate.Repositories.TodoItemRepository _repository;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        // MySQLコンテナの起動
        _mysqlContainer = new MySqlBuilder()
            .WithImage("mysql:8.0")
            .WithDatabase("compatibilitytest_newerapplication")
            .WithUsername("testuser")
            .WithPassword("testpass")
            .Build();
        await _mysqlContainer.StartAsync();

        // DbContextの設定 (app)
        var appDbOptions = new DbContextOptionsBuilder<ExampleEFCoreMigrate.Models.Contexts.ApplicationDbContext>()
            .UseMySql(_mysqlContainer.GetConnectionString(), new MySqlServerVersion(new Version(8, 0, 43)))
            .Options;
        _dbContext = new ExampleEFCoreMigrate.Models.Contexts.ApplicationDbContext(appDbOptions);

        // DbContextの設定 (db)
        var migrateDbOptions = new DbContextOptionsBuilder<NullableApplicationDbContext>()
            .UseMySql(_mysqlContainer.GetConnectionString(), new MySqlServerVersion(new Version(8, 0, 43)))
            .Options;
        var migrateDbContext = new NullableApplicationDbContext(migrateDbOptions);

        // EnsureCreatedAsync でスキーマ内に DbContext 相当内容を作成してくれる
        await migrateDbContext.Database.EnsureCreatedAsync();

        // リポジトリの初期化
        _repository = new ExampleEFCoreMigrate.Repositories.TodoItemRepository(_dbContext);
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _mysqlContainer.DisposeAsync();
    }

    [SetUp]
    public async Task SetUp()
    {
        // テスト前にDBをクリア
        _dbContext.TodoItems.RemoveRange(_dbContext.TodoItems);
        await _dbContext.SaveChangesAsync();
    }

    [Test]
    public async Task AddAsync_追加したTodoItemが取得できる()
    {
        var todo = new ExampleEFCoreMigrate.Models.Entities.TodoItem { Title = "テスト", IsCompleted = false, Timestamp = DateTime.UtcNow };
        await _repository.AddAsync(todo);

        var items = await _repository.GetAllAsync();
        Assert.That(items, Has.Exactly(1).Items);
        Assert.That(items.First().Title, Is.EqualTo("テスト"));
    }

    [Test]
    public async Task GetByIdAsync_存在するIDならTodoItem取得できる()
    {
        var todo = new ExampleEFCoreMigrate.Models.Entities.TodoItem { Title = "IDテスト", IsCompleted = false, Timestamp = DateTime.UtcNow };
        await _repository.AddAsync(todo);
        var id = todo.Id;

        var result = await _repository.GetByIdAsync(id);
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Title, Is.EqualTo("IDテスト"));
    }

    [Test]
    public async Task UpdateAsync_内容とタイムスタンプが更新される()
    {
        var todo = new ExampleEFCoreMigrate.Models.Entities.TodoItem { Title = "更新前", IsCompleted = false, Timestamp = DateTime.UtcNow };
        await _repository.AddAsync(todo);
        var oldTimestamp = todo.Timestamp;

        todo.Title = "更新後";
        await _repository.UpdateAsync(todo);

        var updated = await _repository.GetByIdAsync(todo.Id);
        Assert.That(updated!.Title, Is.EqualTo("更新後"));
        Assert.That(updated.Timestamp, Is.GreaterThan(oldTimestamp));
    }

    [Test]
    public async Task DeleteAsync_削除後は取得できない()
    {
        var todo = new ExampleEFCoreMigrate.Models.Entities.TodoItem { Title = "削除テスト", IsCompleted = false, Timestamp = DateTime.UtcNow };
        await _repository.AddAsync(todo);
        var id = todo.Id;

        await _repository.DeleteAsync(id);
        var result = await _repository.GetByIdAsync(id);
        Assert.That(result, Is.Null);
    }
}
