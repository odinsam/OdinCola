using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Cola.CaCache.IColaCache;
using Cola.CaUtils;
using Cola.CaUtils.Constants;
using Cola.CaUtils.Enums;
using Cola.CaUtils.Helper;
using Cola.Models.Core.Models;
using Cola.Models.Core.Models.CaJwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace WebApplication1Test;

public class ColaIAlwaysRunResultFilter : IAlwaysRunResultFilter
{
    public ColaIAlwaysRunResultFilter(IConfiguration configuration,IColaHybridCache colaHybridCache)
    {
    }
    public void OnResultExecuting(ResultExecutingContext context)
    {
    }

    public void OnResultExecuted(ResultExecutedContext context)
    {
    }
    
    
    
    // /// <summary>
    // /// 验证token
    // /// </summary>
    // /// <returns></returns>
    // public bool ValidateToken(string token,RefreshToken refreshToken)
    // {
    //     var secretKey = jwtOption.Secret;
    //     if (refreshToken == null)
    //     {
    //         // refreshToken为null...
    //         throw new ColaException(EnumException.RefreshTokenIsNull);
    //     }
    //     var tokenValidationParameters = new TokenValidationParameters
    //         {
    //             ValidateIssuer = false,
    //             ValidateAudience = false,
    //             ValidateIssuerSigningKey = true,
    //             IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
    //             ClockSkew = TimeSpan.Zero,
    //             ValidateLifetime = false // 不验证过期时间！！！
    //         };
    //     
    //         var jwtTokenHandler = new JwtSecurityTokenHandler();
    //         var validateClaimsPrincipal =
    //             jwtTokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
    //     
    //         var validatedSecurityAlgorithm = validatedToken is JwtSecurityToken jwtSecurityToken
    //                                          && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
    //                                              StringComparison.InvariantCultureIgnoreCase);
    //     
    //         var claimsPrincipal = validatedSecurityAlgorithm ? validateClaimsPrincipal : null;
    //         if (claimsPrincipal == null)
    //         {
    //             // 无效的token...
    //             throw new ColaException(EnumException.InvalidToken);
    //         }
    //         var expiryDateUnix =
    //             long.Parse(claimsPrincipal.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
    //         var expiryDateTimeUtc = UnixTimeHelper.GetDateTime(expiryDateUnix.ToString());
    //         if (expiryDateTimeUtc < DateTime.Now)
    //         {
    //             // token过期...
    //             throw new ColaException(EnumException.TokenExpire);
    //         }
    //         var jti = claimsPrincipal.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
    //         
    //         if (refreshToken == null)
    //         {
    //             // 无效的refresh_token...
    //             throw new ColaException(EnumException.InvalidRefreshToken);
    //         }
    //         if (refreshToken.ExpiryTime < DateTime.Now)
    //         {
    //             // refresh_token已过期...
    //             throw new ColaException(EnumException.RefreshTokenExpire);
    //         }
    //         if (refreshToken.Invalidated)
    //         {
    //             // refresh_token已失效...
    //             throw new ColaException(EnumException.RefreshTokenNullified);
    //         }
    //         if (refreshToken.Used)
    //         {
    //             // refresh_token已使用...
    //             throw new ColaException(EnumException.RefreshTokenUsed);
    //         }
    //         if (refreshToken.JwtId != jti)
    //         {
    //             // token 与 refresh_token不一致...
    //             throw new ColaException(EnumException.RefreshTokenValidateJwtIdFail);
    //         }
    //         return true;
    // }
}