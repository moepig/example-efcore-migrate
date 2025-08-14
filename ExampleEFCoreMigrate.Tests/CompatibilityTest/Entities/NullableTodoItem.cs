using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// TodoItem に nullable / デフォルト値なしのカラムを追加
/// </summary>
[Table("TodoItems")]
public class NullableTodoItem
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
    /// 適当なカラム nullable / デフォルト値なし
    /// </summary>
    public long? NewColumn { get; set; }
}