using KaraokeReception.Domain.Entities;
using KaraokeReception.Domain.ValueObjects;
using KaraokeReception.Infrastructure.Data;
using KaraokeReception.Infrastructure.DataModels;
using Microsoft.EntityFrameworkCore;

namespace KaraokeReception.Infrastructure.Repositories;

/// <summary>
/// EF Coreを利用して部屋情報を取得するRepository。
/// </summary>
public class RoomRepository : IRoomRepository
{
    private const string BasePriceType = "BASE";

    private readonly KaraokeReceptionDbContext _dbContext;

    public RoomRepository(KaraokeReceptionDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Room>> GetAllAsync()
    {
        var rooms = await _dbContext.Rooms
            .Include(room => room.Machine)
            .Include(room => room.Prices)
            .AsNoTracking()
            .ToListAsync();

        return rooms
            .Select(ToDomain)
            .ToList();
    }

    /// <summary>
    /// DBの部屋データモデルをドメインの部屋Entityへ変換する。
    /// </summary>
    /// <param name="room">DBから取得した部屋データ。</param>
    /// <returns>ドメインで利用する部屋Entity。</returns>
    private static Room ToDomain(RoomDataModel room)
    {
        var basePrice = GetRequiredBasePrice(room);

        return new Room(
            new RoomId(room.RoomId),
            new PersonCount(room.Capacity),
            ToKaraokeMachineType(room.Machine),
            new Money(basePrice.PricePerMinuteNoTax));
    }

    /// <summary>
    /// 部屋料金マスタから必須の基本料金を取得する。
    /// </summary>
    /// <param name="room">料金を取得する部屋データ。</param>
    /// <returns>基本料金のデータモデル。</returns>
    /// <exception cref="InvalidOperationException">
    /// 基本料金が未登録、または重複登録されている場合に発生する。
    /// </exception>
    private static RoomPriceDataModel GetRequiredBasePrice(RoomDataModel room)
    {
        var basePrices = room.Prices
            .Where(price => price.PriceType == BasePriceType)
            .ToList();

        return basePrices.Count switch
        {
            1 => basePrices[0],
            0 => throw new InvalidOperationException(
                $"部屋ID {room.RoomId} の基本料金が登録されていません。"),
            _ => throw new InvalidOperationException(
                $"部屋ID {room.RoomId} の基本料金が重複して登録されています。")
        };
    }

    /// <summary>
    /// DBの機種マスタをドメインのカラオケ機種へ変換する。
    /// </summary>
    /// <param name="machine">DBから取得した機種データ。</param>
    /// <returns>ドメインで利用するカラオケ機種。</returns>
    /// <exception cref="InvalidOperationException">未対応の機種名の場合に発生する。</exception>
    private static KaraokeMachineType ToKaraokeMachineType(
        MachineDataModel? machine)
    {
        return machine?.MachineName switch
        {
            "DAM" => KaraokeMachineType.Dam,
            "JOYSOUND" => KaraokeMachineType.Joysound,
            _ => throw new InvalidOperationException("未対応のカラオケ機種です。")
        };
    }
}
