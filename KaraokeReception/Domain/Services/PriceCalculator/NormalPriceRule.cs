using KaraokeReception.Domain.Entities;
using KaraokeReception.Domain.ValueObjects;

namespace KaraokeReception.Domain.Services.PriceCalculator;

/// <summary>
/// 通常料金を計算する料金ルール。
/// </summary>
public class NormalPriceRule : IPriceRule
{
    /// <summary>
    /// 大人、学生、シニアの人数区分ごとの単価を使って通常料金を計算する。
    /// </summary>
    /// <param name="usagePlan">予約確定前の部屋利用計画。</param>
    /// <returns>通常料金を適用した場合の金額。</returns>
    public Money Calculate(RoomUsagePlan usagePlan)
    {
        int normalPersonCount = usagePlan.PersonCount.Value - usagePlan.StudentCount - usagePlan.SeniorCount;
        int studentCount = usagePlan.StudentCount;
        int seniorCount = usagePlan.SeniorCount;
        var prices = usagePlan.Room.Prices;

        int normalPrice = normalPersonCount * prices.GetPrice(PriceType.Base).AmountNoTax * usagePlan.UsageTime.TotalMinutes;

        int studentPrice = studentCount * prices.GetPrice(PriceType.Student).AmountNoTax * usagePlan.UsageTime.TotalMinutes;

        int seniorPrice = seniorCount * prices.GetPrice(PriceType.Senior).AmountNoTax * usagePlan.UsageTime.TotalMinutes;

        return new Money(
            normalPrice + studentPrice + seniorPrice
        );
    }
}
