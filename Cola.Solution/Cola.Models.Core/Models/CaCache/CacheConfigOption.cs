namespace Cola.Models.Core.Models.CaCache;

public class CacheConfigOption
{
    /// <summary>
    /// 缓存类型
    /// NoCache = 0,    不使用缓存
    /// InMemory = 1,   使用本地缓存
    /// Redis = 2,      使用redis缓存
    /// Hybrid = 3      优先模式：有限使用本地缓存，再使用redis缓存
    /// </summary>
    public int CacheType { get; set; } = 0;
    /// <summary>
    /// redis 缓存配置
    /// </summary>
    public RedisCacheConfig? RedisCache { get; set; }
    
    public MemoryCacheConfig? MemoryCache { get; set; }
}