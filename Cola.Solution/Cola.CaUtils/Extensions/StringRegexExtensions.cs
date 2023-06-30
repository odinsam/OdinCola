using System.Text.RegularExpressions;

namespace Cola.CaUtils.Extensions;

/// <summary>
///     StringRegexExtensions
/// </summary>
public static class StringRegexExtensions
{
    /// <summary>
    ///     中国身份号码
    /// </summary>
    /// <returns></returns>
    public static string RegexIp4 { get; set; } =
        @"\b(([01]?\d?\d|2[0-4]\d|25[0-5])\.){3}([01]?\d?\d|2[0-4]\d|25[0-5])\b";

    /// <summary>
    ///     中国身份号码
    /// </summary>
    /// <returns></returns>
    public static string RegexChinaCardId { get; set; } = "(\\d{10})\\d{7}([\\dxX]{1})";

    /// <summary>
    ///     身份证掩码规则
    /// </summary>
    /// <value></value>
    public static string StringMarkChinaCardId { get; set; } = "$1*******$2";

    /// <summary>
    ///     中国移动电话号码
    /// </summary>
    /// <returns></returns>
    public static string RegexChinaMobile { get; set; } = "(\\d{3})\\d{4}(\\d{4})";

    /// <summary>
    ///     中国移动电话号码掩码规则
    /// </summary>
    /// <returns></returns>
    public static string StringMarkChinaMobile { get; set; } = "$1****$2";

    /// <summary>
    ///     邮箱
    /// </summary>
    /// <returns></returns>
    public static string RegexEmail { get; set; } =
        @"^([a-zA-Z0-9_-])+.*([a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+((\.[a-zA-Z0-9_-]{2,3}){1,2})$";

    /// <summary>
    ///     ip地址
    /// </summary>
    /// <returns></returns>
    public static string RegexIpAddress { get; set; } = @"(\.((2(5[0-5]|[0-4]\d))|[0-1]?\d{1,2})){3}";

    /// <summary>
    ///     Uri
    /// </summary>
    /// <returns></returns>
    public static string RegexUri { get; set; } =
        @"(https?|ftp|file)://[-A-Za-z0-9+&@#/%?=~_|!:,.;]+[-A-Za-z0-9+&@#/%=~_|]";

    /// <summary>
    ///     判断中国身份号码格式
    /// </summary>
    /// <param name="cardId">cardId</param>
    /// <param name="pattern">pattern</param>
    /// <returns>true if the value parameter is cardId; otherwise, false.</returns>
    public static bool IsChinaCardId(this string cardId, string? pattern = null)
    {
        return Regex.IsMatch(cardId, pattern ?? RegexChinaCardId);
    }

    /// <summary>
    ///     判断中国移动电话号码格式
    /// </summary>
    /// <param name="mobile">mobile</param>
    /// <param name="pattern">pattern</param>
    /// <returns>true if the value parameter is mobile; otherwise, false.</returns>
    public static bool IsChinaMobile(this string mobile, string? pattern = null)
    {
        return Regex.IsMatch(mobile, pattern ?? RegexChinaMobile);
    }

    /// <summary>
    ///     判断Ip地址格式
    /// </summary>
    /// <param name="ipAddress">ipAddress</param>
    /// <param name="pattern">pattern</param>
    /// <returns>true if the value parameter is ipAddress; otherwise, false.</returns>
    public static bool IsIpAddress(this string ipAddress, string? pattern = null)
    {
        return Regex.IsMatch(ipAddress, pattern ?? RegexIpAddress);
    }

    /// <summary>
    ///     判断邮箱地址格式
    /// </summary>
    /// <param name="email">邮箱地址</param>
    /// <param name="pattern">pattern</param>
    /// <returns>true if the value parameter is email; otherwise, false.</returns>
    public static bool IsEmail(this string email, string? pattern = null)
    {
        return Regex.IsMatch(email, pattern ?? RegexEmail);
    }

    /// <summary>
    ///     判断uri格式
    /// </summary>
    /// <param name="uri">uri</param>
    /// <param name="pattern"></param>
    /// <returns>true if the value parameter is uri; otherwise, false.</returns>
    public static bool IsUri(this string uri, string? pattern = null)
    {
        return Regex.IsMatch(uri, pattern ?? RegexUri);
    }

    /// <summary>
    ///     判断ip4格式
    /// </summary>
    /// <param name="ip">ip</param>
    /// <param name="pattern">pattern</param>
    /// <returns></returns>
    public static string IsIp4(this string ip, string? pattern = null)
    {
        var match = Regex.Match(ip, pattern ?? RegexIp4);
        return match.Value;
    }
}