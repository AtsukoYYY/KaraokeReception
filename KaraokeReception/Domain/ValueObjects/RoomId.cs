namespace KaraokeReception.Domain.ValueObjects;

/// <summary>
/// カラオケルームを識別するID。
/// 
/// 形式:
/// 1桁目  : G(地上階) / U(地下階)
/// 2〜3桁 : 階数（例: 03 = 3階）
/// 4〜6桁 : 部屋番号（例: 005 = 005室）
/// </summary>
public readonly record struct RoomId
{
    /// <summary>
    /// 部屋IDの文字列値
    /// </summary>
    public string Value { get; }

    public RoomId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(
                "部屋IDは空にできません。",
                nameof(value));
        }

        if (value.Length != 6)
        {
            throw new ArgumentException(
                "部屋IDは6文字で指定してください。",
                nameof(value));
        }

        // 1文字目は地上階(G)または地下階(U)のみ許可する
        if (value[0] is not ('G' or 'U'))
        {
            throw new ArgumentException(
                "部屋IDの1文字目はG（地上階）またはU（地下階）で指定してください。",
                nameof(value));
        }

        // 2〜3文字目は階数を表すため、数字である必要がある
        if (!int.TryParse(value.AsSpan(1, 2), out int floor))
        {
            throw new ArgumentException(
                "部屋IDの2〜3文字目には階数を表す数字を指定してください。",
                nameof(value));
        }

        // 00階は存在しないため許可しない
        if (floor == 0)
        {
            throw new ArgumentException(
                "階数は1以上で指定してください。",
                nameof(value));
        }

        // 4〜6文字目は部屋番号を表すため、数字である必要がある
        if (!int.TryParse(value.AsSpan(3, 3), out int roomNumber))
        {
            throw new ArgumentException(
                "部屋IDの4〜6文字目には部屋番号を表す数字を指定してください。",
                nameof(value));
        }

        Value = value;
    }

    /// <summary>
    /// 地下階かどうかを判定する
    /// </summary>
    public bool IsUnderground => Value[0] == 'U';

    /// <summary>
    /// 階数を取得する
    /// </summary>
    public int Floor =>
        int.Parse(Value.AsSpan(1, 2));

    /// <summary>
    /// 部屋番号を取得する
    /// </summary>
    public int RoomNumber =>
        int.Parse(Value.AsSpan(3, 3));

    /// <summary>
    /// 表示用の部屋名を取得する
    /// </summary>
    public string ToRoomName()
    {
        string floorType = IsUnderground ? "地下" : "";
        return $"{floorType}{Floor}階{RoomNumber:D3}号室";
    }
}