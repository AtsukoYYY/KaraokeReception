using KaraokeReception.Domain.Entities;

namespace KaraokeReception.Infrastructure.Repositories;

/// <summary>
/// 部屋情報を取得するRepository。
/// </summary>
public interface IRoomRepository
{
    /// <summary>
    /// すべての部屋を取得する。
    /// </summary>
    /// <returns>部屋一覧。</returns>
    Task<IReadOnlyList<Room>> GetAllAsync();
}
