using KaraokeReception.Infrastructure.DataModels;
using Microsoft.EntityFrameworkCore;

namespace KaraokeReception.Infrastructure.Data;

/// <summary>
/// カラオケ予約システムのDBコンテキスト。
/// </summary>
public class KaraokeReceptionDbContext : DbContext
{
    public KaraokeReceptionDbContext(
        DbContextOptions<KaraokeReceptionDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// 部屋マスタ。
    /// </summary>
    public DbSet<RoomDataModel> Rooms => Set<RoomDataModel>();

    /// <summary>
    /// カラオケ機種マスタ。
    /// </summary>
    public DbSet<MachineDataModel> Machines => Set<MachineDataModel>();

    /// <summary>
    /// 部屋料金マスタ。
    /// </summary>
    public DbSet<RoomPriceDataModel> RoomPrices => Set<RoomPriceDataModel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RoomDataModel>(entity =>
        {
            entity.ToTable("m_room");
            entity.HasKey(room => room.RoomId);

            entity.Property(room => room.RoomId)
                .HasColumnName("room_id");

            entity.Property(room => room.MachineId)
                .HasColumnName("machine_id");

            entity.Property(room => room.Capacity)
                .HasColumnName("capacity");

            entity.HasOne(room => room.Machine)
                .WithMany(machine => machine.Rooms)
                .HasForeignKey(room => room.MachineId);
        });

        modelBuilder.Entity<MachineDataModel>(entity =>
        {
            entity.ToTable("m_machine");
            entity.HasKey(machine => machine.MachineId);

            entity.Property(machine => machine.MachineId)
                .HasColumnName("machine_id");

            entity.Property(machine => machine.MachineName)
                .HasColumnName("machine_name");
        });

        modelBuilder.Entity<RoomPriceDataModel>(entity =>
        {
            entity.ToTable("m_room_price");
            entity.HasKey(price => new
            {
                price.RoomId,
                price.PriceType
            });

            entity.Property(price => price.RoomId)
                .HasColumnName("room_id");

            entity.Property(price => price.PriceType)
                .HasColumnName("price_type");

            entity.Property(price => price.PricePerMinuteNoTax)
                .HasColumnName("price_per_minute_no_tax");

            entity.HasOne(price => price.Room)
                .WithMany(room => room.Prices)
                .HasForeignKey(price => price.RoomId);
        });
    }
}
