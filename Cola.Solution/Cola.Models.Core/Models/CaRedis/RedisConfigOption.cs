namespace Cola.Models.Core.Models.CaRedis;

public class RedisConfigOption
{
    /// <summary>
    ///     哨兵服务器
    /// </summary>
    public string? SentinelConnection { get; set; }

    /// <summary>
    ///     读写分离
    /// </summary>
    public bool RwSplitting { get; set; }

    public List<RedisConnection> Connections { get; set; } = null!;
}