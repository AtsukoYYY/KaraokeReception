namespace KaraokeReception.Domain.ValueObjects;

/// <summary>
/// 金額を表す値オブジェクト
/// </summary>
public readonly record struct Money
{
    /// <summary>
    /// 金額（税抜）
    /// </summary>
    public int AmountNoTax { get; }

    /// <summary>
    /// 金額（税込）
    /// </summary>
    public int AmountIncludeTax => (int)Math.Round(AmountNoTax * 1.1);

    /// <summary>
    /// 税
    /// </summary>
    public int AmountTax => AmountIncludeTax - AmountNoTax;


    /// <summary>
    /// 金額を表す値オブジェクトを生成する
    /// </summary>
    /// <param name="amountNoTax">税抜金額</param>
    /// <exception cref="ArgumentException"></exception>
    public Money(int amountNoTax)
    {
        if (amountNoTax < 0)
        {
            throw new ArgumentException(
                "金額は0円以上で指定してください。",
                nameof(amountNoTax));
        }

        AmountNoTax = amountNoTax;
    }

    /// <summary>
    /// 金額表示用文字列を取得する（税抜）
    /// </summary>
    public string ToStringNoTax()
    {
        return $"{AmountNoTax:N0}円";
    }

    /// <summary>
    /// 金額表示用文字列を取得する（税込）
    /// </summary>
    public string ToStringIncludeTax()
    {
        return $"{AmountIncludeTax:N0}円";
    }

    /// <summary>
    /// 金額表示用文字列を取得する（税）
    /// </summary>
    public string ToStringTax()
    {
        return $"{AmountTax:N0}円";
    }

    /// <summary>
    /// 金額の加算
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public Money Add(Money other)
    {
        return new Money(AmountNoTax + other.AmountNoTax);
    }
}