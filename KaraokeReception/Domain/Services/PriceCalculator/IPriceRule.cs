using KaraokeReception.Domain.Entities;
using KaraokeReception.Domain.ValueObjects;

namespace KaraokeReception.Domain.Services.PriceCalculator;

/// <summary>
/// 料金計算ルールのインターフェース
/// </summary>
public interface IPriceRule
{
    /// <summary>
    /// この料金ルールを適用した場合の料金を計算する
    /// </summary>
    /// <param name="usagePlan">予約確定前の部屋利用計画。</param>
    /// <returns>計算結果の金額</returns>
    Money Calculate(RoomUsagePlan usagePlan);
}
