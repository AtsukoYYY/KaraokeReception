using KaraokeReception.Domain.Entities;
using KaraokeReception.Domain.ValueObjects;
namespace KaraokeReception.Domain.Services;

public class NormalPriceRule : IPriceRule
{
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