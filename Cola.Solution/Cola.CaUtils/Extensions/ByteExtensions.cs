using System.Text;

namespace Cola.CaUtils.Extensions;

/// <summary>
///     ByteExtends
/// </summary>
public static class ByteExtensions
{
    /// <summary>
    ///     bytes to string
    /// </summary>
    /// <param name="bytes">byte array</param>
    /// <returns>string</returns>
    public static string GetStringFromBytes(this byte[] bytes)
    {
        return Encoding.Default.GetString(bytes);
    }

    /// <summary>
    ///     将一个byte数组转换成一个格式化的16进制字符串
    /// </summary>
    /// <param name="data">byte数组</param>
    /// <returns>格式化的16进制字符串</returns>
    public static string ConvertByteArrayToHexString(this byte[] data)
    {
        var sb = new StringBuilder(data.Length * 3);
        foreach (var b in data)
            //16进制数字
            sb.Append(Convert.ToString(b, 16).PadLeft(2, '0'));
        //16进制数字之间以空格隔开
        //sb.Append(Convert.ToString(b, 16).PadLeft(2, '0').PadRight(3, ' '));
        return sb.ToString().ToLower();
    }
}