namespace KaraokeReception.Domain.ValueObjects;

/// <summary>
/// 予約を識別するID
/// </summary>
public readonly record struct ReservationId
{
    public string Value { get; }

    public ReservationId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(
                "予約IDは空にできません。",
                nameof(value));
        }

        Value = value;
    }

    public override string ToString() => Value;
}