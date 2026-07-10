namespace KaraokeReception.Infrastructure.DataModels;

/// <summary>
/// m_machine テーブルのデータモデル。
/// </summary>
public class MachineDataModel
{
    /// <summary>
    /// カラオケ機種ID。
    /// </summary>
    public int MachineId { get; set; }

    /// <summary>
    /// カラオケ機種名。
    /// </summary>
    public string MachineName { get; set; } = string.Empty;

    /// <summary>
    /// この機種を利用する部屋一覧。
    /// </summary>
    public List<RoomDataModel> Rooms { get; set; } = [];
}
