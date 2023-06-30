using Cola.CaUtils.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Cola.CaEF.Tenant;

/// <summary>
///     RouteValue 获取 租户id策略
///     在RouteValue中带有 TenantId 参数
/// </summary>
public class RouteValueTenantResolutionStrategy : ITenantResolutionStrategy
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RouteValueTenantResolutionStrategy(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    ///     路由模式 通过字段{tenantId} 传递参数  租户id
    /// </summary>
    /// <returns></returns>
    public string? GetTenantResolutionKey()
    {
        var routeData = _httpContextAccessor.HttpContext?.GetRouteData();
        return routeData?.Values["tenantId"]?.ToString();
    }

    public string ResolveTenantKey()
    {
        var key = GetTenantResolutionKey();
        if (key.IsNullOrEmpty()) throw new ArgumentException("路由中缺少参数 {tenantId} 租户id");
        if (!int.TryParse(key, out var tenantId)) throw new ArgumentException("租户id无法转为int类型");
        return tenantId.ToString();
    }
}