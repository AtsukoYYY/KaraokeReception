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
        return new Room(
            new RoomId(room.RoomId),
            new PersonCount(room.Capacity),
            ToKaraokeMachineType(room.Machine),
            ToRoomPriceTable(room));
    }

    /// <summary>
    /// DBの部屋料金マスタをドメインの料金表へ変換する。
    /// </summary>
    /// <param name="room">料金表を作成する部屋データ。</param>
    /// <returns>ドメインで利用する部屋料金表。</returns>
    /// <exception cref="InvalidOperationException">
    /// 基本料金が未登録の場合、または料金種別が重複している場合に発生する。
    /// </exception>
    private static RoomPriceTable ToRoomPriceTable(RoomDataModel room)
    {
        var duplicatedPriceTypes = room.Prices
            .GroupBy(price => price.PriceType)
            .Where(group => group.Count() > 1)
            .Select(group => group.Key)
            .ToList();

        if (duplicatedPriceTypes.Count > 0)
        {
            throw new InvalidOperationException(
                $"部屋ID {room.RoomId} の料金種別が重複して登録されています。"
                + $" 対象: {string.Join(", ", duplicatedPriceTypes)}");
        }

        var hasBasePrice = room.Prices
            .Any(price => price.PriceType == PriceType.Base.Value);

        if (!hasBasePrice)
        {
            throw new InvalidOperationException(
                $"部屋ID {room.RoomId} の基本料金が登録されていません。");
        }

        var prices = room.Prices
            .ToDictionary(
                price => new PriceType(price.PriceType),
                price => new Money(price.PricePerMinuteNoTax));

        return new RoomPriceTable(prices);
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
