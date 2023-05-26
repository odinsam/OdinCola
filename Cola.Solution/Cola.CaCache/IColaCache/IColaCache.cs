namespace Cola.CaCache.IColaCache;

public interface IColaCacheBase
{
    /// <summary>
    ///     Get Object
    /// </summary>
    /// <param name="key">key</param>
    /// <param name="database">database index default 0</param>
    /// <typeparam name="T">object type</typeparam>
    /// <returns>Object</returns>
    T? Get<T>(string key, int database = 0);

    /// <summary>
    ///     set object
    /// </summary>
    /// <param name="key">key</param>
    /// <param name="value">object.ToJson()</param>
    /// <param name="expiry">expiry</param>
    /// <param name="database">database index default 0</param>
    /// <typeparam name="T">object type</typeparam>
    /// <returns>true is success,otherwise false</returns>
    bool Set<T>(string key, T value, TimeSpan? expiry = null, int database = 0);
}