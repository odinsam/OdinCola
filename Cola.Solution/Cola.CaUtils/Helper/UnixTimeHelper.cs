using Cola.CaUtils.Enums;

namespace Cola.CaUtils.Helper;

public class UnixTimeHelper
{
    private static readonly DateTime BaseTime = new(1970, 1, 1);

    /// <summary>
    ///     将unixtime转换为.NET的DateTime
    /// </summary>
    /// <param name="dateTime">秒数</param>
    /// <param name="ssOrMs">秒数</param>
    /// <returns>转换后的时间</returns>
    private static long FromDateTime(DateTime dateTime, EnumSsOrMs? ssOrMs = EnumSsOrMs.Ss)
    {
        if (ssOrMs == null || ssOrMs == EnumSsOrMs.Ss)
            return FromDateTime(dateTime);
        return (dateTime.ToLocalTime().Ticks - BaseTime.ToLocalTime().Ticks) / 10000;
    }
    
    /// <summary>  
    /// Unix时间戳转为C#格式时间  
    /// </summary>  
    /// <param name="timeStamp">Unix时间戳格式,例如1482115779</param>  
    /// <returns>C#格式时间</returns>  
    public static DateTime GetDateTime(string timeStamp)
    {
        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(BaseTime);
        long lTime = long.Parse(timeStamp + "0000000");
        TimeSpan toNow = new TimeSpan(lTime);
        return dtStart.Add(toNow);
    }

    /// <summary>
    ///     将.NET的DateTime转换为unix time
    /// </summary>
    /// <param name="dateTime">待转换的时间</param>
    /// <returns>转换后的unix time</returns>
    public static long FromDateTime(DateTime dateTime)
    {
        return (dateTime.ToLocalTime().Ticks - BaseTime.ToLocalTime().Ticks) / 10000000;
    }

    /// <summary>
    ///     将.NET的DateTime转换为unix time
    /// </summary>
    /// <returns>转换后的unix time</returns>
    public static long GetUnixDateTime()
    {
        return FromDateTime(DateTime.Now);
    }
    
    /// <summary>
    ///     将.NET的DateTime转换为unix time ms
    /// </summary>
    /// <returns>转换后的unix time</returns>
    public static long GetUnixDateTimeMs()
    {
        return FromDateTime(DateTime.Now, EnumSsOrMs.Ms);
    }
}