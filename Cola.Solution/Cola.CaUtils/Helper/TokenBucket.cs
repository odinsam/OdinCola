namespace Cola.CaUtils.Helper;
/*
 * 调用

private readonly TokenBucket _tokenBucket = new TokenBucket(10, 1); // 桶容量为 10，每秒补充 1 个令牌


if (!_tokenBucket.TryAcquire())
{
    throw new Exception("当前没有可访问的令牌");
}

 */
/// <summary>
/// 令牌桶算法
/// </summary>
public class TokenBucket
{
    /// <summary>
    /// 当前剩余令牌数量
    /// </summary>
    public double Tokens
    {
        get
        {
            return _tokens;
        }
    }
    private readonly int _capacity;  // 桶的容量
    private readonly double _refillRate; // 令牌的补充速率
    private double _tokens; // 当前令牌数量
    private DateTime _lastRefillTime; // 上次补充令牌的时间

    public TokenBucket(int capacity, double refillRate)
    {
        _capacity = capacity;
        _refillRate = refillRate;
        _tokens = capacity;
        _lastRefillTime = DateTime.UtcNow;
    }
    
    /// <summary>
    /// 尝试获取令牌
    /// </summary>
    /// <returns></returns>
    public bool TryAcquire()
    {
        RefillTokens();

        lock (this)
        {
            if (_tokens >= 1)
            {
                _tokens--;
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// 添加令牌
    /// </summary>
    private void RefillTokens()
    {
        var currentTime = DateTime.UtcNow;
        var timeElapsed = (currentTime - _lastRefillTime).TotalSeconds;
        var tokensToAdd = timeElapsed * _refillRate;
        if (tokensToAdd > 0)
        {
            lock (this)
            {
                _tokens = Math.Min(_tokens + tokensToAdd, _capacity);
                _lastRefillTime = currentTime;
            }
        }
    }
}