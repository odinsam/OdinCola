using Cola.CaUtils.Enums;
using Cola.CaUtils.Helper;

namespace Cola.CaUtils.Extensions;

/// <summary>
///     TimeExtensions
/// </summary>
public static class TimeExtensions
{
    private static readonly DateTime BaseTime = new(1970, 1, 1);

    /// <summary>
    ///     ToUnixTime时间戳
    /// </summary>
    /// <param name="now">DateTime now</param>
    /// <returns>时间戳</returns>
    public static long ToUnixTime(this DateTime now)
    {
        return UnixTimeHelper.GetUnixDateTime();
    }

    /// <summary>
    ///     ToUnixTime时间戳 (毫秒级别)
    /// </summary>
    /// <param name="now">DateTime now</param>
    /// <returns>时间戳</returns>
    public static long ToUnixTimeMs(this DateTime now)
    {
        return UnixTimeHelper.GetUnixDateTimeMs();
    }

    /// <summary>
    ///     时间戳反转为时间，有很多中翻转方法，但是，请不要使用过字符串（string）进行操作，大家都知道字符串会很慢！
    /// </summary>
    /// <param name="timeStamp">时间戳</param>
    /// <param name="ssOrMs">是否精确到毫秒</param>
    /// <returns>返回一个日期时间</returns>
    public static DateTime ToTimer(this long timeStamp, EnumSsOrMs? ssOrMs = EnumSsOrMs.Ss)
    {
        if (ssOrMs == EnumSsOrMs.Ms)
            return BaseTime.ToLocalTime().AddTicks(timeStamp * 10000);
        return BaseTime.ToLocalTime().AddTicks(timeStamp * 10000000);
    }

    /// <summary>
    ///     时间差返回 MM:dd HH:mm / hh:mm
    /// </summary>
    /// <returns></returns>
    public static string TimeDifferenceToString(this long longTs, long dtNow)
    {
        var ts = new TimeSpan(longTs);
        var now = dtNow.ToTimer();
        if (ts.Days > 0)
        {
            now = now.AddDays(0 - ts.Days);
            return now.ToString("MM月dd日 hh:mm");
        }

        return now.ToString("hh:mm");
    }
}