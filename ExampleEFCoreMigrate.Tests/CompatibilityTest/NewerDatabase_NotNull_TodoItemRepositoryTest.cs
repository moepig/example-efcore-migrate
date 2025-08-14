using ExampleEFCoreMigrate.Models.Contexts;
using ExampleEFCoreMigrate.Models.Entities;
using ExampleEFCoreMigrate.Repositories;
using ExampleEFCoreMigrate.Tests.CompatibilityTest.Contexts;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MySql;

namespace ExampleEFCoreMigrate.Tests.CompatibilityTest;

[TestFixture]
public class NewerDatabase_NotNull_TodoItemRepositoryTest
{
    private MySqlContainer _mysqlContainer;
    private ApplicationDbContext _dbContext;
    private TodoItemRepository _repository;

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
        var appDbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseMySql(_mysqlContainer.GetConnectionString(), new MySqlServerVersion(new Version(8, 0, 43)))
            .Options;
        _dbContext = new ApplicationDbContext(appDbOptions);

        // DbContextの設定 (db)
        var migrateDbOptions = new DbContextOptionsBuilder<NotNullApplicationDbContext>()
            .UseMySql(_mysqlContainer.GetConnectionString(), new MySqlServerVersion(new Version(8, 0, 43)))
            .Options;
        var migrateDbContext = new NotNullApplicationDbContext(migrateDbOptions);

        // EnsureCreatedAsync でスキーマ内に DbContext 相当内容を作成してくれる
        await migrateDbContext.Database.EnsureCreatedAsync();

        // リポジトリの初期化
        _repository = new TodoItemRepository(_dbContext);
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
    public async Task AddAsync_デフォルト値が未設定なので失敗する()
    {
        var todo = new TodoItem { Title = "テスト", IsCompleted = false, Timestamp = DateTime.UtcNow };

        try
        {
            await _repository.AddAsync(todo);
            Assert.Fail("例外が発生しませんでした");
        }
        catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
        {
            Assert.That(ex.InnerException, Is.TypeOf<MySqlConnector.MySqlException>());

            if (ex.InnerException is MySqlConnector.MySqlException mySqlEx)
            {
                Assert.That(mySqlEx.Message, Does.Contain("Field 'NewColumn' doesn't have a default value"));

                // Server Error Code が 1364 であること
                Assert.That(mySqlEx.Number, Is.EqualTo(1364));
            }
        }
    }
}
