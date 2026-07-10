using KaraokeReception.Domain.Entities;
using KaraokeReception.Domain.ValueObjects;

namespace KaraokeReception.Domain.Services.PriceCalculator;

/// <summary>
/// 早朝割を適用した料金を計算する料金ルール。
/// </summary>
public class EarlyDiscountRule : IPriceRule
{
    /// <summary>
    /// 早朝割対象時間は早朝料金、それ以外の時間は基本料金で計算する。
    /// </summary>
    /// <param name="usagePlan">予約確定前の部屋利用計画。</param>
    /// <returns>早朝割を適用した場合の金額。</returns>
    public Money Calculate(RoomUsagePlan usagePlan)
    {
        var prices = usagePlan.Room.Prices;

        // 早朝割引料金計算
        return new Money(
            usagePlan.PersonCount.Value *
            (prices.GetPrice(PriceType.Early).AmountNoTax * usagePlan.UsageTime.EarlyMorningMinutes
            + prices.GetPrice(PriceType.Base).AmountNoTax * usagePlan.UsageTime.NormalMinutes)
        );
    }
}
