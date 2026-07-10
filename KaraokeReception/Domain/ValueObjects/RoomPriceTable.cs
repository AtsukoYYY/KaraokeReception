namespace KaraokeReception.Domain.ValueObjects;

/// <summary>
/// 部屋に設定された料金表を表す値オブジェクト。
/// </summary>
public class RoomPriceTable
{
    private readonly IReadOnlyDictionary<PriceType, Money> _prices;

    /// <summary>
    /// 部屋の料金表を生成する。
    /// </summary>
    /// <param name="prices">料金種別ごとの1人1分あたりの税抜料金。</param>
    /// <exception cref="ArgumentException">基本料金が含まれていない場合に発生する。</exception>
    public RoomPriceTable(
        IReadOnlyDictionary<PriceType, Money> prices)
    {
        if (!prices.ContainsKey(PriceType.Base))
        {
            throw new ArgumentException(
                "部屋料金には基本料金が必要です。",
                nameof(prices));
        }

        _prices = prices;
    }

    /// <summary>
    /// 料金種別に対応する1人1分あたりの税抜料金を取得する。
    /// </summary>
    /// <remarks>
    /// 指定した料金種別が登録されていない場合は、基本料金を返す。
    /// </remarks>
    /// <param name="priceType">取得する料金種別。</param>
    /// <returns>1人1分あたりの税抜料金。</returns>
    public Money GetPrice(PriceType priceType)
    {
        if (_prices.TryGetValue(priceType, out var price))
        {
            return price;
        }

        return _prices[PriceType.Base];
    }
}
