using KaraokeReception.Domain.Entities;
using KaraokeReception.Domain.ValueObjects;

namespace KaraokeReception.Domain.Services;

/// <summary>
/// 利用可能な料金ルールから最適な料金を計算するサービス
/// </summary>
public class PriceCalculator
{
    private readonly IEnumerable<IPriceRule> _priceRules;

    public PriceCalculator(
        IEnumerable<IPriceRule> priceRules)
    {
        _priceRules = priceRules;
    }

    /// <summary>
    /// 最も安い料金を計算する
    /// </summary>
    public Money Calculate(Reservation reservation)
    {
        return _priceRules
            .Select(rule => rule.Calculate(reservation))
            .MinBy(price => price.AmountNoTax);
    }
}