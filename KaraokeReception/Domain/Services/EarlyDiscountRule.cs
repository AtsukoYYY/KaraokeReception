using KaraokeReception.Domain.Entities;
using KaraokeReception.Domain.ValueObjects;

namespace KaraokeReception.Domain.Services;

/// <summary>
/// 早朝割を適用した料金を計算する料金ルール。
/// </summary>
public class EarlyDiscountRule : IPriceRule
{
    /// <summary>
    /// 早朝割対象時間は早朝料金、それ以外の時間は基本料金で計算する。
    /// </summary>
    /// <param name="reservation">料金計算対象の予約。</param>
    /// <returns>早朝割を適用した場合の金額。</returns>
    public Money Calculate(Reservation reservation)
    {
        // 早朝割引料金計算
        return new Money(
            reservation.PersonCount.Value *
            (reservation.Room.EarlyTimePrice.AmountNoTax * reservation.UsageTime.EarlyMorningMinutes
            + reservation.Room.BasePrice.AmountNoTax * reservation.UsageTime.NormalMinutes)
        );
    }
}
