using KaraokeReception.Domain.Entities;
using KaraokeReception.Domain.ValueObjects;

namespace KaraokeReception.Domain.Services.PriceCalculator;

/// <summary>
/// 利用可能な料金ルールから最適な料金を計算するサービス
/// </summary>
public class PriceCalculator
{
    private readonly IEnumerable<IPriceRule> _priceRules;

    /// <summary>
    /// 利用可能な料金ルールを受け取って、料金計算サービスを生成する。
    /// </summary>
    /// <param name="priceRules">料金候補を計算するルール一覧。</param>
    public PriceCalculator(
        IEnumerable<IPriceRule> priceRules)
    {
        _priceRules = priceRules;
    }

    /// <summary>
    /// 最も安い料金を計算する
    /// </summary>
    /// <param name="usagePlan">予約確定前の部屋利用計画。</param>
    /// <returns>利用可能な料金ルールの中で最も安い金額。</returns>
    public Money Calculate(RoomUsagePlan usagePlan)
    {
        return _priceRules
            .Select(rule => rule.Calculate(usagePlan))
            .MinBy(price => price.AmountNoTax);
    }
}
