using KaraokeReception.Domain.Entities;
using KaraokeReception.Domain.ValueObjects;
namespace KaraokeReception.Domain.Services;

public class EarlyDiscountRule : IPriceRule
{
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