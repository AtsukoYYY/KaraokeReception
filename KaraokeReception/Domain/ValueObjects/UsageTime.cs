namespace KaraokeReception.Domain.ValueObjects;

/// <summary>
/// カラオケ利用時間を表す値オブジェクト
/// </summary>
public readonly record struct UsageTime
{
    // 早朝割対象時間
    private const int EarlyMorningStartHour = 6;
    private const int EarlyMorningEndHour = 10;

    private const int MaxUsageMinutes = 1440;

    /// <summary>
    /// 利用開始日時
    /// </summary>
    public DateTime Start { get; }

    /// <summary>
    /// 利用終了日時
    /// </summary>
    public DateTime End { get; }

    /// <summary>
    /// 合計利用時間（分）
    /// </summary>
    public int TotalMinutes =>
        (int)(End - Start).TotalMinutes;


    /// <summary>
    /// 早朝割対象時間（分）
    /// </summary>
    public int EarlyMorningMinutes
    {
        get
        {
            var earlyStart = Start.Date.AddHours(EarlyMorningStartHour);
            var earlyEnd = Start.Date.AddHours(EarlyMorningEndHour);

            var overlapStart = Start > earlyStart ? Start : earlyStart;
            var overlapEnd = End < earlyEnd ? End : earlyEnd;

            if (overlapEnd <= overlapStart)
            {
                return 0;
            }

            return (int)(overlapEnd - overlapStart).TotalMinutes;
        }
    }

    /// <summary>
    /// 早朝割対象外時間（分）
    /// </summary>
    public int NormalMinutes =>
        TotalMinutes - EarlyMorningMinutes;


    public UsageTime(
        DateTime start,
        DateTime end)
    {
        // 秒以下は予約時間として不要なため切り捨てる
        Start = new DateTime(
            start.Year,
            start.Month,
            start.Day,
            start.Hour,
            start.Minute,
            0);

        End = new DateTime(
            end.Year,
            end.Month,
            end.Day,
            end.Hour,
            end.Minute,
            0);

        if (End <= Start)
        {
            throw new ArgumentException(
                "終了日時は開始日時より後にしてください。");
        }

        if (TotalMinutes > MaxUsageMinutes)
        {
            throw new ArgumentException(
                "利用時間は24時間以内にしてください。");
        }
    }
}