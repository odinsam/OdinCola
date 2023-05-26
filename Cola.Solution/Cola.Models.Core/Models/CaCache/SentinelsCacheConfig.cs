namespace Cola.Models.Core.Models.CaCache;

/// <summary>
///     哨兵+集群模式
/// </summary>
public class SentinelsCacheConfig : RedisCacheConfig
{
    public string? Sentinel { get; set; }
    public string? Masters { get; set; }
    public string? Slaves { get; set; }
    public string? ServiceName { get; set; }
}