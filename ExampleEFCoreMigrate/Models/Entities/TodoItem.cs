using System.ComponentModel.DataAnnotations;

namespace ExampleEFCoreMigrate.Models.Entities;

/// <summary>
/// TODO アイテムを表すモデル
/// </summary>
public class TodoItem
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
}