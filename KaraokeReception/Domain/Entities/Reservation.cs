using KaraokeReception.Domain.ValueObjects;

namespace KaraokeReception.Domain.Entities;

/// <summary>
/// カラオケルームの予約を表す
/// </summary>
public class Reservation
{
    /// <summary>
    /// 予約ID
    /// </summary>
    public ReservationId Id { get; }

    /// <summary>
    /// 予約された部屋利用計画。
    /// </summary>
    public RoomUsagePlan UsagePlan { get; }

    /// <summary>
    /// 予約IDと部屋利用計画から予約を生成する。
    /// </summary>
    /// <param name="id">予約ID。</param>
    /// <param name="usagePlan">予約された部屋利用計画。</param>
    public Reservation(
        ReservationId id,
        RoomUsagePlan usagePlan)
    {
        Id = id;
        UsagePlan = usagePlan;
    }
}
