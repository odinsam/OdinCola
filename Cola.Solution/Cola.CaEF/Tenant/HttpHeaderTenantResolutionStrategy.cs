using Microsoft.AspNetCore.Http;

namespace Cola.CaEF.Tenant;

/// <summary>
///     HttpHeader 获取 租户id策略
///     在HttpHeader中带有 TenantId 参数
/// </summary>
public class HttpHeaderTenantResolutionStrategy : ITenantResolutionStrategy
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpHeaderTenantResolutionStrategy(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    ///     httpHeader模式 固定 传递参数  TenantId
    /// </summary>
    /// <returns></returns>
    public string? GetTenantResolutionKey()
    {
        return "tenantId";
    }

    /// <summary>
    ///     通过 httpHeader 中 TenantId 来获取当前请求的的租户Id
    /// </summary>
    /// <returns></returns>
    public string ResolveTenantKey()
    {
        if (!int.TryParse(_httpContextAccessor.HttpContext?.Request?.Headers[GetTenantResolutionKey()].ToString(),
                out var tenantId)) throw new ArgumentException("httpHeader 中 缺少参数 TenantId租户id或租户id无法转为int类型");
        return tenantId.ToString();
    }
}