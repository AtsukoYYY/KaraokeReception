namespace KaraokeReception.ViewModels.Home;

/// <summary>
/// 空室検索画面の表示モデル
/// </summary>
public class RoomSearchViewModel
{
    /// <summary>
    /// 空室検索フォームの入力値。
    /// </summary>
    public RoomSearchInputModel Input { get; set; } = new();
}
