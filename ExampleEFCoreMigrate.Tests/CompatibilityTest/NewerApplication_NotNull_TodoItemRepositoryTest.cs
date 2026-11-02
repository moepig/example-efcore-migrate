using ExampleEFCoreMigrate.Tests.CompatibilityTest.Contexts;
using ExampleEFCoreMigrate.Tests.CompatibilityTest.Entities;
using ExampleEFCoreMigrate.Tests.CompatibilityTest.Repositories;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MySql;

namespace ExampleEFCoreMigrate.Tests.CompatibilityTest;

[TestFixture]
public class NewerApplication_NotNull_TodoItemRepositoryTest
{
    private MySqlContainer _mysqlContainer;
    private NotNullApplicationDbContext _dbContext;
    private NotNullTodoItemRepository _repository;

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
        var appDbOptions = new DbContextOptionsBuilder<NotNullApplicationDbContext>()
            .UseMySql(_mysqlContainer.GetConnectionString(), new MySqlServerVersion(new Version(8, 0, 43)))
            .Options;
        _dbContext = new NotNullApplicationDbContext(appDbOptions);

        // DbContextの設定 (db)
        var migrateDbOptions = new DbContextOptionsBuilder<Models.Contexts.ApplicationDbContext>()
        .UseMySql(_mysqlContainer.GetConnectionString(), new MySqlServerVersion(new Version(8, 0, 43)))
        .Options;
        var migrateDbContext = new Models.Contexts.ApplicationDbContext(migrateDbOptions);

        // EnsureCreatedAsync でスキーマ内に DbContext 相当内容を作成してくれる
        await migrateDbContext.Database.EnsureCreatedAsync();

        // リポジトリの初期化
        _repository = new NotNullTodoItemRepository(_dbContext);
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _mysqlContainer.DisposeAsync();
    }

    [Test]
    public async Task AddAsync_カラムが存在しないので失敗する()
    {
        var todo = new NotNullTodoItem { Title = "テスト", IsCompleted = false, Timestamp = DateTime.UtcNow, NewColumn = 100 };
        
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
                Assert.That(mySqlEx.Message, Does.Contain("Unknown column 'NewColumn' in 'field list'"));

                // Server Error Code が 1054 であること
                Assert.That(mySqlEx.Number, Is.EqualTo(1054));
            }
        }
    }
}
