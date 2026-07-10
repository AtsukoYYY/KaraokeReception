using KaraokeReception.Domain.ValueObjects;

namespace KaraokeReception.Domain.Entities;

/// <summary>
/// 部屋
/// </summary>
public class Room(
    RoomId id,
    PersonCount capacity,
    KaraokeMachineType machineType,
    RoomPriceTable prices)
{
    /// <summary>
    /// 部屋ID
    /// </summary>
    public RoomId Id { get; } = id;

    /// <summary>
    /// 部屋の定員
    /// </summary>
    public PersonCount Capacity { get; } = capacity;

    /// <summary>
    /// カラオケ機種
    /// </summary>
    public KaraokeMachineType MachineType { get; } = machineType;

    /// <summary>
    /// 部屋に設定された料金表。
    /// </summary>
    public RoomPriceTable Prices { get; } = prices;

    /// <summary>
    /// 利用者向けの部屋名を取得する
    /// </summary>
    public string Name => Id.ToRoomName();
}
