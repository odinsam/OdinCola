using System.ComponentModel;

namespace Cola.CaUtils.Extensions;

/// <summary>
///     ColaEnumExtends
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    ///     toInt
    /// </summary>
    /// <param name="str">string</param>
    /// <returns></returns>
    public static int ToInt(this Enum str)
    {
        return Convert.ToInt32(str);
    }

    /// <summary>
    ///     Convert enums to List:String
    /// </summary>
    /// <param name="enums"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IEnumerable<string> ConvertEnumsToEnumerabls<T>(this IEnumerable<T> enums)
    {
        var strs = new List<string>();
        foreach (var item in enums) strs.Add(item!.ToString()!);
        return strs;
    }

    /// <summary>
    ///     get enum description
    /// </summary>
    /// <param name="enumValue">enum</param>
    /// <returns>enum description</returns>
    public static string GetDescription(this Enum enumValue)
    {
        var value = enumValue.ToString();
        var field = enumValue.GetType().GetField(value);
        if (field != null)
        {
            var objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false); //获取描述属性
            if (objs.Length == 0) //当描述属性没有时，直接返回名称
                return value;
            var descriptionAttribute = (DescriptionAttribute)objs[0];
            return descriptionAttribute.Description;
        }

        throw new Exception($"{enumValue} 无法找到对应的 DescriptionAttribute");
    }

    /// <summary>
    ///     Parse Object to Enum
    /// </summary>
    /// <param name="oValue"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T? ParseEnum<T>(this object? oValue)
    {
        if (oValue == null) return default;
        if (oValue.Equals('\0')) return (T)Convert.ChangeType(0, typeof(T));
        if (oValue is T) return (T)oValue;
        var sValue = oValue.ToString();
        if (string.IsNullOrEmpty(sValue))
            return default;
        try
        {
            var o = Enum.Parse(typeof(T), sValue);
            return (T)o;
        }
        catch
        {
            return default;
        }
    }
}