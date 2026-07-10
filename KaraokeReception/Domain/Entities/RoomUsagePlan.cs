using KaraokeReception.Domain.Entities;
using KaraokeReception.Domain.ValueObjects;

namespace KaraokeReception.Domain.Entities;

/// <summary>
/// 部屋利用計画。
/// </summary>
/// <remarks>
/// 検索結果表示の段階では予約確定前の利用計画として扱い、
/// 予約確定後は <see cref="Reservation"/> に紐づく利用内容として扱う。
/// </remarks>
public class RoomUsagePlan
{
    /// <summary>
    /// 利用する部屋。
    /// </summary>
    public required Room Room { get; init; }

    /// <summary>
    /// 利用時間。
    /// </summary>
    public required UsageTime UsageTime { get; init; }

    /// <summary>
    /// 利用人数。
    /// </summary>
    public required PersonCount PersonCount { get; init; }

    /// <summary>
    /// 学生人数。
    /// </summary>
    public int StudentCount { get; init; }

    /// <summary>
    /// シニア人数。
    /// </summary>
    public int SeniorCount { get; init; }
}
