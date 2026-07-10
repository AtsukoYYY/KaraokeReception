namespace KaraokeReception.Domain.ValueObjects;

/// <summary>
/// 部屋料金の種別を表す値オブジェクト。
/// </summary>
/// <param name="Value">料金種別を識別する値。</param>
public readonly record struct PriceType(string Value)
{
    /// <summary>
    /// 基本料金。
    /// </summary>
    public static PriceType Base => new("BASE");

    /// <summary>
    /// 学生料金。
    /// </summary>
    public static PriceType Student => new("STUDENT");

    /// <summary>
    /// シニア料金。
    /// </summary>
    public static PriceType Senior => new("SENIOR");

    /// <summary>
    /// 早朝料金。
    /// </summary>
    public static PriceType Early => new("EARLY");

    /// <summary>
    /// 表示用文字列を取得する。
    /// </summary>
    /// <returns>料金種別を識別する値。</returns>
    public override string ToString()
    {
        return Value;
    }
}
