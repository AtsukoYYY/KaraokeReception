namespace KaraokeReception.ViewModels.Home;

/// <summary>
/// 空室検索結果に表示する部屋情報
/// </summary>
public class AvailableRoomViewModel
{
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
    /// 見込み料金の税抜表示。
    /// </summary>
    public required string EstimatedPriceNoTax { get; init; }

    /// <summary>
    /// 見込み料金の税込表示。
    /// </summary>
    public required string EstimatedPriceIncludeTax { get; init; }
}
