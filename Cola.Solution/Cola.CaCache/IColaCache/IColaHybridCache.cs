using Cola.CaCache.ColaCache;
using Microsoft.Extensions.Caching.Memory;
using StackExchange.Redis;

namespace Cola.CaCache.IColaCache;

public interface IColaHybridCache : IColaCacheBase
{
    public  IColaRedisCache ColaRedisCache{ get; set; }
    public  IColaMemoryCache ColaMemoryCache{ get; set; }    

    /// <summary>
    ///     Refresh object
    /// </summary>
    /// <param name="key">key</param>
    /// <param name="expiry">expiry</param>
    /// <param name="database">database index default 0</param>
    void Refresh(string key, TimeSpan? expiry = null, int database = 0);

    /// <summary>
    ///     Remove object
    /// </summary>
    /// <param name="key">key</param>
    /// <param name="database">database index default 0</param>
    /// <returns>true is success,otherwise false</returns>
    bool Remove(string key, int database = 0);

    /// <summary>
    ///     get RedisLock object
    /// </summary>
    /// <param name="key">lock key</param>
    /// <param name="expiry">lock expiry</param>
    /// <param name="database">database index default 0</param>
    /// <returns>RedisLock object</returns>
    RedisLock RedisLock(string key, TimeSpan expiry, int database = 0);
    
    /// <summary>
    ///     get MemoryLock object
    /// </summary>
    ReaderWriterLockSlim MemoryLock();
    

    /// <summary>
    /// RedisLockMethodAsync
    /// </summary>
    /// <param name="key">lock key</param>
    /// <param name="expiry">lock expiry</param>
    /// <param name="success">success method(writeConnection,readConnection,eventBusConnection)</param>
    /// <param name="fail">fail method(writeConnection,readConnection,eventBusConnection,Exception)</param>
    /// <param name="database">lock database</param>
    void RedisLockMethodAsync(string key, TimeSpan expiry,
        Action<ConnectionMultiplexer, ConnectionMultiplexer, ConnectionMultiplexer> success,
        Action<ConnectionMultiplexer, ConnectionMultiplexer, ConnectionMultiplexer, Exception> fail, int database = 0);
    
    /// <summary>
    /// RedisLockMethodAsync
    /// </summary>
    /// <param name="success">success method(IMemoryCache)</param>
    /// <param name="fail">fail method(IMemoryCache,Exception)</param>
    void MemoryLockMethodAsync<T>(Action<IMemoryCache> success, Action<IMemoryCache, Exception> fail);

    /// <summary>
    ///     订阅
    /// </summary>
    /// <param name="channel">channel</param>
    /// <param name="handler">handler</param>
    void Subscribe(string channel, Action<string, string> handler);

    /// <summary>
    ///     发布
    /// </summary>
    /// <param name="channel">channel</param>
    /// <param name="message">message</param>
    void Publish(string channel, string message);

    /// <summary>
    ///     取消订阅
    /// </summary>
    /// <param name="channel">channel</param>
    void Unsubscribe(string channel);
}