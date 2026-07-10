namespace KaraokeReception.ViewModels.Home;

/// <summary>
/// 予約完了画面の表示モデル。
/// </summary>
public class ReservationCompleteViewModel
{
    /// <summary>
    /// 発行された予約ID。
    /// </summary>
    public required string ReservationId { get; init; }

    /// <summary>
    /// 表示用の予約ID。
    /// </summary>
    public required string ReservationDisplayId { get; init; }

    /// <summary>
    /// 予約した部屋ID。
    /// </summary>
    public required string RoomId { get; init; }

    /// <summary>
    /// 予約した部屋名。
    /// </summary>
    public required string RoomName { get; init; }

    /// <summary>
    /// 表示用の利用時間合計。
    /// </summary>
    public required string TotalUsageTime { get; init; }

    /// <summary>
    /// 見込み料金の税抜表示。
    /// </summary>
    public required string EstimatedPriceNoTax { get; init; }

    /// <summary>
    /// 見込み料金の税込表示。
    /// </summary>
    public required string EstimatedPriceIncludeTax { get; init; }
}
