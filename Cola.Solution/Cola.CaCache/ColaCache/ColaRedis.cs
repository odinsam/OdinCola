using Cola.CaCache.IColaCache;
using Cola.CaUtils.Extensions;
using Cola.Models.Core.Models.CaCache;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Cola.CaCache.ColaCache;

public class ColaRedis : IColaRedisCache, IDisposable
{
    public ConnectionMultiplexer EventBusRedis { get; set; }
    public ConnectionMultiplexer MastersWriteRedis { get; set; }
    public ConnectionMultiplexer SlavesReadRedis { get; set; }
    public ISubscriber Subscriber{ get; set; }

    public ColaRedis(CacheConfigOption options)
    {
        if (options.CacheType == CacheType.Redis.ToInt() || options.CacheType == CacheType.Hybrid.ToInt())
        {
            #region 单机服务器

            if (options.RedisCache.Standalone != null && !options.RedisCache.Standalone.ConnectionStrings.IsNullOrEmpty())
            {
                var redisOptions = ConfigurationOptions.Parse(options.RedisCache.Standalone.ConnectionStrings);
                MastersWriteRedis = ConnectionMultiplexer.Connect(redisOptions);
            }

            #endregion

            #region 哨兵服务器

            if (options.RedisCache.Sentinels != null && !options.RedisCache.Sentinels.ServiceName.IsNullOrEmpty() &&
                !options.RedisCache.Sentinels.Sentinel.IsNullOrEmpty())
            {
                var sentinelEndpoints = ConfigurationOptions.Parse(options.RedisCache.Sentinels.Sentinel);
                var sentinelRedisOptions = new ConfigurationOptions
                {
                    ServiceName = options.RedisCache.Sentinels.ServiceName ?? "Sentinel",
                    TieBreaker = sentinelEndpoints.TieBreaker ?? "",
                    CommandMap = sentinelEndpoints.CommandMap,
                    EndPoints = sentinelEndpoints.EndPoints,
                    AllowAdmin = sentinelEndpoints.AllowAdmin,
                    Password = sentinelEndpoints.Password,
                    DefaultDatabase = sentinelEndpoints.DefaultDatabase ?? 0,
                    AsyncTimeout = sentinelEndpoints.AsyncTimeout == 0 ? 5000 : sentinelEndpoints.AsyncTimeout,
                    SyncTimeout = sentinelEndpoints.SyncTimeout == 0 ? 5000 : sentinelEndpoints.SyncTimeout,
                    ConnectTimeout = sentinelEndpoints.ConnectTimeout == 0 ? 5000 : sentinelEndpoints.ConnectTimeout,
                    ConnectRetry = sentinelEndpoints.ConnectRetry == 0 ? 3 : sentinelEndpoints.ConnectRetry,
                    AbortOnConnectFail = sentinelEndpoints.AbortOnConnectFail,
                    KeepAlive = sentinelEndpoints.KeepAlive == 0 ? 180 : sentinelEndpoints.KeepAlive
                };
                MastersWriteRedis = ConnectionMultiplexer.SentinelConnect(sentinelRedisOptions);
            }

            #endregion

            #region 集群服务器

            if (options.RedisCache.Cluster != null && !options.RedisCache.Cluster.Masters.IsNullOrEmpty() &&
                !options.RedisCache.Cluster.Slaves.IsNullOrEmpty())
            {
                var mastersEndpoints = ConfigurationOptions.Parse(options.RedisCache.Cluster.Masters);
                var slavesEndpoints = ConfigurationOptions.Parse(options.RedisCache.Cluster.Slaves);
                var mastersRedisOptions = new ConfigurationOptions
                {
                    TieBreaker = mastersEndpoints.TieBreaker ?? "",
                    CommandMap = mastersEndpoints.CommandMap,
                    EndPoints = mastersEndpoints.EndPoints,
                    AllowAdmin = mastersEndpoints.AllowAdmin,
                    Password = mastersEndpoints.Password,
                    DefaultDatabase = mastersEndpoints.DefaultDatabase ?? 0,
                    AsyncTimeout = mastersEndpoints.AsyncTimeout == 0 ? 5000 : mastersEndpoints.AsyncTimeout,
                    SyncTimeout = mastersEndpoints.SyncTimeout == 0 ? 5000 : mastersEndpoints.SyncTimeout,
                    ConnectTimeout = mastersEndpoints.ConnectTimeout == 0 ? 5000 : mastersEndpoints.ConnectTimeout,
                    ConnectRetry = mastersEndpoints.ConnectRetry == 0 ? 3 : mastersEndpoints.ConnectRetry,
                    AbortOnConnectFail = mastersEndpoints.AbortOnConnectFail,
                    KeepAlive = mastersEndpoints.KeepAlive == 0 ? 180 : mastersEndpoints.KeepAlive
                };
                MastersWriteRedis = ConnectionMultiplexer.Connect(mastersRedisOptions);
                var slavesRedisOptions = new ConfigurationOptions
                {
                    TieBreaker = slavesEndpoints.TieBreaker ?? "",
                    CommandMap = slavesEndpoints.CommandMap,
                    EndPoints = slavesEndpoints.EndPoints,
                    AllowAdmin = slavesEndpoints.AllowAdmin,
                    Password = slavesEndpoints.Password,
                    DefaultDatabase = slavesEndpoints.DefaultDatabase ?? 0,
                    AsyncTimeout = slavesEndpoints.AsyncTimeout == 0 ? 5000 : slavesEndpoints.AsyncTimeout,
                    SyncTimeout = slavesEndpoints.SyncTimeout == 0 ? 5000 : slavesEndpoints.SyncTimeout,
                    ConnectTimeout = slavesEndpoints.ConnectTimeout == 0 ? 5000 : slavesEndpoints.ConnectTimeout,
                    ConnectRetry = slavesEndpoints.ConnectRetry == 0 ? 3 : slavesEndpoints.ConnectRetry,
                    AbortOnConnectFail = slavesEndpoints.AbortOnConnectFail,
                    KeepAlive = slavesEndpoints.KeepAlive == 0 ? 180 : slavesEndpoints.KeepAlive
                };
                SlavesReadRedis = ConnectionMultiplexer.Connect(slavesRedisOptions);
            }

            #endregion

            #region 订阅发布事件总线

            if (!options.RedisCache.EventBus.IsNullOrEmpty())
            {
                var eventBusRedisOptions = ConfigurationOptions.Parse(options.RedisCache.EventBus);
                EventBusRedis = ConnectionMultiplexer.Connect(eventBusRedisOptions);
            }

            #endregion
            if (SlavesReadRedis == null) SlavesReadRedis = MastersWriteRedis;
        }
    }
    
    public T? Get<T>(string key, int database = 0)
    {
        var db = SlavesReadRedis.GetDatabase(database);
        var value = db.StringGet(key);
        return value.HasValue ? JsonConvert.DeserializeObject<T>(value.ToString()) : default;
    }
    
    public DateTime GetExpireTime(string key, int database = 0)
    {
        var db = SlavesReadRedis.GetDatabase(database);
        return db.KeyExpireTime(key).Value;
    }

    public bool Set<T>(string key, T value, TimeSpan? expiry = null, int database = 0)
    {
        var db = MastersWriteRedis.GetDatabase(database);
        return db.StringSet(key, value!.ToJson(), expiry);
    }

    public void Refresh(string key, TimeSpan? expiry = null, int database = 0)
    {
        var db = MastersWriteRedis.GetDatabase(database);
        db.KeyExpire(key, expiry);
    }

    public bool Remove(string key, int database = 0)
    {
        var db = MastersWriteRedis.GetDatabase(database);
        return db.KeyDelete(key);
    }

    public RedisLock RedisLock(string key, TimeSpan expiry, int database = 0)
    {
        return new RedisLock(MastersWriteRedis.GetDatabase(database), key, expiry);
    }
    
    public async void RedisLockMethodAsync(string key, TimeSpan expiry,Action<ConnectionMultiplexer,ConnectionMultiplexer,ConnectionMultiplexer> success,Action<ConnectionMultiplexer,ConnectionMultiplexer,ConnectionMultiplexer,Exception> fail, int database = 0)
    {
        var writeDb = MastersWriteRedis.GetDatabase(database);
        var redisLocked = new RedisLock(writeDb, key, expiry); 
        if (await redisLocked.Acquire())
        {
            try
            {
                success(MastersWriteRedis,SlavesReadRedis,EventBusRedis);
            }
            catch (Exception e)
            {
                fail(MastersWriteRedis,SlavesReadRedis,EventBusRedis,e);
            }
            finally
            {
                await redisLocked.Unlock();
            }
        }
    }

    public void Subscribe(string channel, Action<string, string> handler)
    {
        Subscriber = EventBusRedis.GetSubscriber();
        Subscriber.Subscribe(channel, (redisChannel, value) => handler(redisChannel, value));
    }

    public void Publish(string channel, string message)
    {
        var publisher = EventBusRedis.GetSubscriber();
        publisher.Publish(channel, message);
    }

    public void Unsubscribe(string channel)
    {
        if (Subscriber != null) Subscriber.Unsubscribe(channel);
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}