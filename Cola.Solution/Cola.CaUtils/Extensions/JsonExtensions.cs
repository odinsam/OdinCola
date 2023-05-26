using Cola.CaUtils.Helper;
using Newtonsoft.Json.Linq;

namespace Cola.CaUtils.Extensions;

public static class JsonExtensions
{
    /// <summary>
    ///     JArray To JObjectList
    /// </summary>
    /// <param name="jary"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static JObjectList<T> ConvertJArrayToJObjectList<T>(this JArray? jary)
    {
        return new JObjectList<T>(jary);
    }

    /// <summary>
    ///     ConvertJTokenToJObjectList
    /// </summary>
    /// <param name="jtoken"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static JObjectList<T> ConvertJTokenToJObjectList<T>(this JToken jtoken)
    {
        return new JObjectList<T>(jtoken as JArray);
    }

    /// <summary>
    ///     JObject add key/value
    /// </summary>
    /// <param name="jObj"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    public static void Add<T>(this JObject jObj, string key, JObjectList<T> value)
    {
        jObj.Add(key, value.ToJArray());
    }
}