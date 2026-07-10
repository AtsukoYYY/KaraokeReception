using System.Security.Cryptography;

namespace KaraokeReception.Domain.ValueObjects;

/// <summary>
/// 予約を識別するID
/// </summary>
public readonly record struct ReservationId
{
    private const int ReservationIdLength = 12;
    private const int DisplayBlockLength = 4;
    private const string AvailableChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    /// <summary>
    /// 予約IDの内部値。
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// 予約IDを生成する。
    /// </summary>
    /// <param name="value">12桁の半角英数字。</param>
    /// <exception cref="ArgumentException">予約IDの形式が不正な場合に発生する。</exception>
    public ReservationId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(
                "予約IDは空にできません。",
                nameof(value));
        }

        var normalizedValue = value.Replace("-", string.Empty).ToUpperInvariant();

        if (normalizedValue.Length != ReservationIdLength)
        {
            throw new ArgumentException(
                "予約IDは12桁で指定してください。",
                nameof(value));
        }

        if (normalizedValue.Any(x => !AvailableChars.Contains(x)))
        {
            throw new ArgumentException(
                "予約IDは半角英数字で指定してください。",
                nameof(value));
        }

        Value = normalizedValue;
    }

    /// <summary>
    /// 新しい予約IDを発行する。
    /// </summary>
    /// <returns>新しい予約ID。</returns>
    public static ReservationId New()
    {
        var chars = Enumerable.Range(0, ReservationIdLength)
            .Select(_ => AvailableChars[RandomNumberGenerator.GetInt32(AvailableChars.Length)])
            .ToArray();

        return new ReservationId(new string(chars));
    }

    /// <summary>
    /// 4桁区切りの表示用予約IDを取得する。
    /// </summary>
    /// <returns>表示用の予約ID。</returns>
    public string ToDisplayString()
    {
        var value = Value;

        return string.Join(
            "-",
            Enumerable.Range(0, ReservationIdLength / DisplayBlockLength)
                .Select(index => value.Substring(
                    index * DisplayBlockLength,
                    DisplayBlockLength)));
    }

    public override string ToString() => Value;
}
