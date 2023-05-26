using StackExchange.Redis;

namespace Cola.CaCache.ColaCache;

public class RedisLock : IDisposable
{
    private readonly IDatabase _db;
    private readonly TimeSpan _expiry;
    private readonly string _key;
    private readonly string _value;

    public RedisLock(IDatabase db, string key, TimeSpan expiry)
    {
        _db = db;
        _key = key;
        _expiry = expiry;
        _value = Guid.NewGuid().ToString();
    }

    /// <summary>
    ///     Dispose 释放锁
    /// </summary>
    public async void Dispose()
    {
        await Unlock();
    }

    /// <summary>
    ///     获取锁
    /// </summary>
    /// <returns></returns>
    public async Task<bool> Acquire()
    {
        var result = await _db.LockTakeAsync(_key, _value, _expiry);
        return result;
    }

    /// <summary>
    ///     释放锁
    /// </summary>
    public async Task<bool> Unlock()
    {
        return await _db.LockReleaseAsync(_key, _value);
    }
}