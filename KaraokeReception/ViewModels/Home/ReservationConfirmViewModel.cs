namespace KaraokeReception.ViewModels.Home;

/// <summary>
/// 予約確認画面の表示モデル。
/// </summary>
public class ReservationConfirmViewModel
{
    /// <summary>
    /// 予約確認で使用する入力値。
    /// </summary>
    public ReservationConfirmInputModel Input { get; set; } = new();

    /// <summary>
    /// 表示用の予約ID。
    /// </summary>
    public required string ReservationId { get; init; }

    /// <summary>
    /// 部屋ID。
    /// </summary>
    public required string RoomId { get; init; }

    /// <summary>
    /// 利用者向けの部屋名。
    /// </summary>
    public required string RoomName { get; init; }

    /// <summary>
    /// 表示用の定員。
    /// </summary>
    public required string Capacity { get; init; }

    /// <summary>
    /// 表示用のカラオケ機種。
    /// </summary>
    public required string MachineType { get; init; }

    /// <summary>
    /// 表示用の開始時間。
    /// </summary>
    public required string StartTime { get; init; }

    /// <summary>
    /// 表示用の終了時間。
    /// </summary>
    public required string EndTime { get; init; }

    /// <summary>
    /// 見込み料金の税抜表示。
    /// </summary>
    public required string EstimatedPriceNoTax { get; init; }

    /// <summary>
    /// 見込み料金の税込表示。
    /// </summary>
    public required string EstimatedPriceIncludeTax { get; init; }
}
