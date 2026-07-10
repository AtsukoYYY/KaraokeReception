namespace KaraokeReception.Infrastructure.DataModels;

/// <summary>
/// m_room テーブルのデータモデル。
/// </summary>
public class RoomDataModel
{
    /// <summary>
    /// 部屋ID。
    /// </summary>
    public string RoomId { get; set; } = string.Empty;

    /// <summary>
    /// カラオケ機種ID。
    /// </summary>
    public int MachineId { get; set; }

    /// <summary>
    /// 定員。
    /// </summary>
    public int Capacity { get; set; }

    /// <summary>
    /// カラオケ機種。
    /// </summary>
    public MachineDataModel? Machine { get; set; }

    /// <summary>
    /// 部屋ごとの料金一覧。
    /// </summary>
    public List<RoomPriceDataModel> Prices { get; set; } = [];
}
