namespace Cola.CaEF.Tenant;

/// <summary>
///     多租户获取租户ID策略
/// </summary>
public interface ITenantResolutionStrategy
{
    string? GetTenantResolutionKey();
    string ResolveTenantKey();
}