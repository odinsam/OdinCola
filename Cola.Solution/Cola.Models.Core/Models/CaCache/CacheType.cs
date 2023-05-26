namespace Cola.Models.Core.Models.CaCache;

/// <summary>
///     CacheType 缓存类型
/// </summary>
public enum CacheType
{
    /// <summary>
    ///     禁用缓存
    /// </summary>
    NoCache = 0,

    /// <summary>
    ///     使用本地缓存
    /// </summary>
    InMemory = 1,

    /// <summary>
    ///     使用redis缓存
    /// </summary>
    Redis = 2,

    /// <summary>
    ///     使用本地缓存+redis缓存
    /// </summary>
    Hybrid = 3
}