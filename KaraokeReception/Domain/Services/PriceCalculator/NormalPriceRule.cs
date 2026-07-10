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
    /// <param name="reservation">料金計算対象の予約。</param>
    /// <returns>通常料金を適用した場合の金額。</returns>
    public Money Calculate(Reservation reservation)
    {
        int normalPersonCount = reservation.PersonCount.Value - reservation.StudentCount - reservation.SeniorCount;
        int studentCount = reservation.StudentCount;
        int seniorCount = reservation.SeniorCount;

        int normalPrice = normalPersonCount * reservation.Room.BasePrice.AmountNoTax * reservation.UsageTime.TotalMinutes;

        int studentPrice = studentCount * reservation.Room.StudentPrice.AmountNoTax * reservation.UsageTime.TotalMinutes;

        int seniorPrice = seniorCount * reservation.Room.SeniorPrice.AmountNoTax * reservation.UsageTime.TotalMinutes;

        return new Money(
            normalPrice + studentPrice + seniorPrice
        );
    }
}
