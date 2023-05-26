using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace WebApplication1Test;

public class IdempotentAttributeFilter : IActionFilter, IResultFilter
{
    private const string IdempotencyKeyHeaderName = "IdempotencyKey";
    private readonly IDistributedCache _distributedCache;
    private string _idempotencyKey;
    private bool _isIdempotencyCache;

    public IdempotentAttributeFilter(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        StringValues idempotencyKeys;
        context.HttpContext.Request.Headers.TryGetValue(IdempotencyKeyHeaderName, out idempotencyKeys);
        _idempotencyKey = idempotencyKeys.ToString();

        var cacheData = _distributedCache.GetString(GetDistributedCacheKey());
        if (cacheData != null)
        {
            context.Result = JsonConvert.DeserializeObject<ObjectResult>(cacheData);
            _isIdempotencyCache = true;
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }

    public void OnResultExecuted(ResultExecutedContext context)
    {
        //已缓存
        if (_isIdempotencyCache) return;

        var contextResult = context.Result;

        var cacheOptions = new DistributedCacheEntryOptions();
        cacheOptions.AbsoluteExpirationRelativeToNow = new TimeSpan(24, 0, 0);

        //缓存:
        _distributedCache.SetString(GetDistributedCacheKey(), JsonConvert.SerializeObject(contextResult), cacheOptions);
    }

    public void OnResultExecuting(ResultExecutingContext context)
    {
    }

    private string GetDistributedCacheKey()
    {
        return "Idempotency:" + _idempotencyKey;
    }
}