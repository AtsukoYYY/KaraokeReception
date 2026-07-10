namespace KaraokeReception.ViewModels.Home;

/// <summary>
/// 空室検索結果画面の表示モデル
/// </summary>
public class RoomSearchResultViewModel
{
    /// <summary>
    /// 検索に使用した予約条件。
    /// </summary>
    public RoomSearchInputModel SearchCondition { get; set; } = new();

    /// <summary>
    /// 表示用の利用時間合計。
    /// </summary>
    public string TotalUsageTime { get; set; } = string.Empty;

    /// <summary>
    /// 条件に合う空室候補。
    /// </summary>
    public IReadOnlyList<AvailableRoomViewModel> AvailableRooms { get; set; } =
        [];
}
