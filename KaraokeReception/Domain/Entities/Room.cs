using KaraokeReception.Domain.ValueObjects;

namespace KaraokeReception.Domain.Entities;

/// <summary>
/// 部屋
/// </summary>
public class Room(
    RoomId id,
    PersonCount capacity,
    KaraokeMachineType machineType,
    Money basePrice)
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
    /// 部屋の基本料金（1人1分あたり）税抜
    /// </summary>
    public Money BasePrice { get; } = basePrice;

    /// <summary>
    /// 部屋の学生割引料金（1人1分あたり）税抜
    /// </summary>
    public Money StudentPrice => new((int)(BasePrice.AmountNoTax * 0.7));

    /// <summary>
    /// 部屋のシニア割引料金（1人1分あたり）税抜
    /// </summary>
    public Money SeniorPrice => new((int)(BasePrice.AmountNoTax * 0.8));
    /// <summary>
    /// 部屋の早朝割引料金（1人1分あたり）税抜
    /// </summary>
    public Money EarlyTimePrice => new((int)(BasePrice.AmountNoTax * 0.5));

    /// <summary>
    /// 利用者向けの部屋名を取得する
    /// </summary>
    public string Name => Id.ToRoomName();
}