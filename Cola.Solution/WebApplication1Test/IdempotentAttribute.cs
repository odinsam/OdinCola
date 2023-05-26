using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;

namespace WebApplication1Test;

public class IdempotentAttribute : Attribute, IFilterFactory
{
    public bool IsReusable => false;

    public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
    {
        var distributedCache = (IDistributedCache)serviceProvider.GetService(typeof(IDistributedCache));
        var filter = new IdempotentAttributeFilter(distributedCache);
        return filter;
    }
}