namespace KaraokeReception.ViewModels.Home;

/// <summary>
/// 予約確認へ進むための入力値。
/// </summary>
public class ReservationConfirmInputModel : RoomSearchInputModel
{
    /// <summary>
    /// 予約ID。
    /// </summary>
    public string ReservationId { get; set; } = string.Empty;

    /// <summary>
    /// 予約対象の部屋ID。
    /// </summary>
    public string RoomId { get; set; } = string.Empty;
}
