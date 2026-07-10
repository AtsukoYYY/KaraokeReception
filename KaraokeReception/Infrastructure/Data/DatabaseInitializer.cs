using KaraokeReception.Infrastructure.DataModels;
using Microsoft.EntityFrameworkCore;

namespace KaraokeReception.Infrastructure.Data;

/// <summary>
/// 開発用DBの作成と初期データ投入を行う。
/// </summary>
public static class DatabaseInitializer
{
    private const string BasePriceType = "BASE";

    /// <summary>
    /// DBが存在しない場合は作成し、部屋検索に必要な初期データを投入する。
    /// </summary>
    /// <param name="dbContext">カラオケ予約システムのDBコンテキスト。</param>
    public static async Task InitializeAsync(
        KaraokeReceptionDbContext dbContext)
    {
        await dbContext.Database.EnsureCreatedAsync();

        if (await dbContext.Rooms.AnyAsync())
        {
            return;
        }

        dbContext.Machines.AddRange(
            new MachineDataModel
            {
                MachineId = 1,
                MachineName = "DAM"
            },
            new MachineDataModel
            {
                MachineId = 2,
                MachineName = "JOYSOUND"
            });

        dbContext.Rooms.AddRange(
            new RoomDataModel
            {
                RoomId = "G03001",
                Capacity = 4,
                MachineId = 1
            },
            new RoomDataModel
            {
                RoomId = "G03005",
                Capacity = 6,
                MachineId = 2
            },
            new RoomDataModel
            {
                RoomId = "G04002",
                Capacity = 8,
                MachineId = 1
            },
            new RoomDataModel
            {
                RoomId = "U01003",
                Capacity = 12,
                MachineId = 2
            });

        dbContext.RoomPrices.AddRange(
            CreateBasePrice("G03001", 12),
            CreateBasePrice("G03005", 14),
            CreateBasePrice("G04002", 18),
            CreateBasePrice("U01003", 22));

        await dbContext.SaveChangesAsync();
    }

    private static RoomPriceDataModel CreateBasePrice(
        string roomId,
        int pricePerMinuteNoTax)
    {
        return new RoomPriceDataModel
        {
            RoomId = roomId,
            PriceType = BasePriceType,
            PricePerMinuteNoTax = pricePerMinuteNoTax
        };
    }
}
