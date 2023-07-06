using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;

namespace Cola.CaUtils.Extensions;

public static class JwtExtensions
{
    public static string GetClaimValue(this IIdentity identity,string valueType)
    {
        var valueObj = identity == null ? null : (identity as ClaimsIdentity)!.Claims.FirstOrDefault(x => x.Type == valueType);
        return valueObj==null? null:valueObj.Value;
    }
}