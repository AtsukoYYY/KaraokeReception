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
    /// 利用する部屋
    /// </summary>
    public Room Room { get; }

    /// <summary>
    /// 利用時間
    /// </summary>
    public UsageTime UsageTime { get; }

    /// <summary>
    /// 利用人数
    /// </summary>
    public PersonCount PersonCount { get; }

    /// <summary>
    /// 学生人数
    /// </summary>
    public int StudentCount { get; }

    /// <summary>
    /// シニア人数
    /// </summary>
    public int SeniorCount { get; }


    public Reservation(
        ReservationId id,
        Room room,
        UsageTime usageTime,
        PersonCount personCount,
        int studentCount,
        int seniorCount)
    {
        Id = id;
        Room = room;
        UsageTime = usageTime;
        PersonCount = personCount;
        StudentCount = studentCount;
        SeniorCount = seniorCount;
    }
}