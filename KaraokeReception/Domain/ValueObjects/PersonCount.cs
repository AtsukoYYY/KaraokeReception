namespace KaraokeReception.Domain.ValueObjects;

public readonly record struct PersonCount
{
    public int Value { get; }

    public PersonCount(int value)
    {
        if (value <= 0)
        {
            throw new ArgumentException(
                "人数は1以上である必要があります。");
        }

        if (value > 100)
        {
            throw new ArgumentException(
                "人数は100以下である必要があります。");
        }

        Value = value;
    }
}