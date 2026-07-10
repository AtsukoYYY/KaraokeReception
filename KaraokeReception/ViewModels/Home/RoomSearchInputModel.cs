using System.ComponentModel.DataAnnotations;

namespace KaraokeReception.ViewModels.Home;

/// <summary>
/// 空室検索フォームの入力値
/// </summary>
public class RoomSearchInputModel
{
    /// <summary>
    /// 大人人数。
    /// </summary>
    [Display(Name = "大人人数")]
    [Range(0, 100, ErrorMessage = "大人人数は0人以上100人以下で入力してください。")]
    public int AdultCount { get; set; }

    /// <summary>
    /// 学生人数。
    /// </summary>
    [Display(Name = "学生人数")]
    [Range(0, 100, ErrorMessage = "学生人数は0人以上100人以下で入力してください。")]
    public int StudentCount { get; set; }

    /// <summary>
    /// シニア人数。
    /// </summary>
    [Display(Name = "シニア人数(65歳以上)")]
    [Range(0, 100, ErrorMessage = "シニア人数は0人以上100人以下で入力してください。")]
    public int SeniorCount { get; set; }

    /// <summary>
    /// 利用開始日時。
    /// </summary>
    [Display(Name = "開始時間")]
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 利用終了日時。
    /// </summary>
    [Display(Name = "終了時間")]
    public DateTime EndTime { get; set; }

    /// <summary>
    /// 大人、学生、シニアを合計した利用人数。
    /// </summary>
    public int TotalPersonCount => AdultCount + StudentCount + SeniorCount;

    /// <summary>
    /// 利用時間合計(時間)
    /// </summary>
    [Display(Name = "利用時間合計")]
    public decimal TotalUsageTime => (decimal)(EndTime - StartTime).TotalHours;
}
