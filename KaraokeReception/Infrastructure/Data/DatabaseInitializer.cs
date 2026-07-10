using KaraokeReception.Domain.ValueObjects;
using KaraokeReception.Infrastructure.DataModels;
using Microsoft.EntityFrameworkCore;

namespace KaraokeReception.Infrastructure.Data;

/// <summary>
/// 開発用DBの作成と初期データ投入を行う。
/// </summary>
public static class DatabaseInitializer
{
    /// <summary>
    /// 開発用DBを作り直し、部屋検索に必要な初期データを投入する。
    /// </summary>
    /// <param name="dbContext">カラオケ予約システムのDBコンテキスト。</param>
    public static async Task InitializeAsync(
        KaraokeReceptionDbContext dbContext)
    {
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

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

        dbContext.RoomPrices.AddRange(CreateSeedRoomPrices());

        await dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// 開発用DBへ投入する部屋料金マスタを作成する。
    /// </summary>
    /// <returns>開発用の部屋料金マスタ一覧。</returns>
    private static IEnumerable<RoomPriceDataModel> CreateSeedRoomPrices()
    {
        yield return CreateRoomPrice(
            "G03001",
            PriceType.Base.Value,
            12);

        yield return CreateRoomPrice(
            "G03001",
            PriceType.Student.Value,
            8);

        yield return CreateRoomPrice(
            "G03001",
            PriceType.Senior.Value,
            9);

        yield return CreateRoomPrice(
            "G03001",
            PriceType.Early.Value,
            6);

        yield return CreateRoomPrice(
            "G03005",
            PriceType.Base.Value,
            14);

        yield return CreateRoomPrice(
            "G03005",
            PriceType.Student.Value,
            10);

        yield return CreateRoomPrice(
            "G03005",
            PriceType.Senior.Value,
            11);

        yield return CreateRoomPrice(
            "G03005",
            PriceType.Early.Value,
            7);

        yield return CreateRoomPrice(
            "G04002",
            PriceType.Base.Value,
            18);

        yield return CreateRoomPrice(
            "G04002",
            PriceType.Student.Value,
            13);

        yield return CreateRoomPrice(
            "G04002",
            PriceType.Senior.Value,
            14);

        yield return CreateRoomPrice(
            "G04002",
            PriceType.Early.Value,
            9);

        yield return CreateRoomPrice(
            "U01003",
            PriceType.Base.Value,
            22);

        yield return CreateRoomPrice(
            "U01003",
            PriceType.Student.Value,
            16);

        yield return CreateRoomPrice(
            "U01003",
            PriceType.Senior.Value,
            18);

        yield return CreateRoomPrice(
            "U01003",
            PriceType.Early.Value,
            11);
    }

    private static RoomPriceDataModel CreateRoomPrice(
        string roomId,
        string priceType,
        int pricePerMinuteNoTax)
    {
        return new RoomPriceDataModel
        {
            RoomId = roomId,
            PriceType = priceType,
            PricePerMinuteNoTax = pricePerMinuteNoTax
        };
    }
}
