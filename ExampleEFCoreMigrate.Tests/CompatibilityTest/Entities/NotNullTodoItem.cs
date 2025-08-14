using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExampleEFCoreMigrate.Tests.CompatibilityTest.Entities;

/// <summary>
/// TodoItem に NOT NULL / デフォルト値なしのカラムを追加
/// </summary>
[Table("TodoItems")]
public class NotNullTodoItem
{
    /// <summary>
    /// ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// タイトル
    /// </summary>
    [Required(ErrorMessage = "タイトルは必須です。")]
    [StringLength(100, ErrorMessage = "タイトルは100文字以内で入力してください。")]
    public required string Title { get; set; }

    /// <summary>
    /// 完了したかどうか
    /// </summary>
    public bool IsCompleted { get; set; }

    /// <summary>
    /// 作成・更新日時
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// 適当なカラム NOT NULL / デフォルト値なし
    /// </summary>
    public long NewColumn { get; set; }
}