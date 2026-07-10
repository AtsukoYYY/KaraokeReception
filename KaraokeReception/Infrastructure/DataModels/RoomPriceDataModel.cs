namespace KaraokeReception.Infrastructure.DataModels;

/// <summary>
/// m_room_price テーブルのデータモデル。
/// </summary>
public class RoomPriceDataModel
{
    /// <summary>
    /// 部屋ID。
    /// </summary>
    public string RoomId { get; set; } = string.Empty;

    /// <summary>
    /// 料金種別。
    /// </summary>
    public string PriceType { get; set; } = string.Empty;

    /// <summary>
    /// 1人1分あたりの税抜料金。
    /// </summary>
    public int PricePerMinuteNoTax { get; set; }

    /// <summary>
    /// 料金が紐づく部屋。
    /// </summary>
    public RoomDataModel? Room { get; set; }
}
