using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Cola.CaUtils;
using Cola.CaUtils.Enums;
using Cola.CaUtils.Extensions;
using Cola.CaUtils.Helper;
using Cola.Models.Core.Models.CaJwt;
using Microsoft.IdentityModel.Tokens;

namespace Cola.CaJwt;

public class TokenHelper
{
    /// <summary>
    /// 验证token
    /// </summary>
    /// <param name="jwtOption">jwtOption</param>
    /// <param name="token">token</param>
    /// <param name="storedRefreshToken">服务端存储的RefreshToken</param>
    /// <returns></returns>
    public static bool ValidateToken(ColaJwtOption jwtOption, string token, RefreshToken? storedRefreshToken)
    {
        var secretKey = jwtOption.Secret;
        if (storedRefreshToken == null)
        {
            // refreshToken为null...
            throw new ColaException(EnumException.RefreshTokenIsNull);
        }
        var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = false // 不验证过期时间！！！
            };
        
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var validateClaimsPrincipal =
                jwtTokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
        
            var validatedSecurityAlgorithm = validatedToken is JwtSecurityToken jwtSecurityToken
                                             && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                                                 StringComparison.InvariantCultureIgnoreCase);
        
            var claimsPrincipal = validatedSecurityAlgorithm ? validateClaimsPrincipal : null;
            if (claimsPrincipal == null)
            {
                // 无效的token...
                throw new ColaException(EnumException.InvalidToken);
            }
            var expiryDateUnix =
                long.Parse(claimsPrincipal.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
            var expiryDateTimeUtc = UnixTimeHelper.GetDateTime(expiryDateUnix.ToString());
            if (expiryDateTimeUtc < DateTime.Now)
            {
                // token过期...
                throw new ColaException(EnumException.TokenExpire);
            }
            var jti = claimsPrincipal.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
            
            if (storedRefreshToken == null)
            {
                // 无效的refresh_token...
                throw new ColaException(EnumException.InvalidRefreshToken);
            }
            if (storedRefreshToken.ExpiryTime < DateTime.Now)
            {
                // refresh_token已过期...
                throw new ColaException(EnumException.RefreshTokenExpire);
            }
            if (storedRefreshToken.Invalidated)
            {
                // refresh_token已失效...
                throw new ColaException(EnumException.RefreshTokenNullified);
            }
            if (storedRefreshToken.Used)
            {
                // refresh_token已使用...
                throw new ColaException(EnumException.RefreshTokenUsed);
            }
            if (storedRefreshToken.JwtId != jti)
            {
                // token 与 refresh_token不一致...
                throw new ColaException(EnumException.RefreshTokenValidateJwtIdFail);
            }
            return true;
    }
    
    /// <summary>
    /// CreateToken
    /// </summary>
    /// <param name="loginName">loginName</param>
    /// <param name="secretKey">secretKey</param>
    /// <param name="expiresMinutes">expiresMinutes token过期时间</param>
    /// <param name="refreshTokenExpiresMinutes">refreshTokenExpiresMinutes 默认 24*60 分钟</param>
    /// <returns></returns>
    public static TokenResult CreateToken(string loginName, string secretKey, int expiresMinutes, int refreshTokenExpiresMinutes=24*60)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var securityKey = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)), SecurityAlgorithms.HmacSha256);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name,loginName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
            }),
            IssuedAt = DateTime.Now,
            NotBefore = DateTime.Now,
            Expires = DateTime.Now.AddMinutes(expiresMinutes),
            SigningCredentials = securityKey
        };
        var securityToken = jwtTokenHandler.CreateToken(tokenDescriptor);
        var token = jwtTokenHandler.WriteToken(securityToken);
        var refreshToken = new RefreshToken() 
        {
            JwtId = securityToken.Id,
            TokenId = loginName.ToMd5Lower(),
            CreationTime = DateTime.Now,
            ExpiryTime = DateTime.Now.AddMonths(refreshTokenExpiresMinutes),
            Token = Convert.ToBase64String(Encoding.UTF8.GetBytes(GenerationCodeHelper.GenerationCode(32,true,true,false)))
        };
        return new TokenResult()
        {
            Token = new AccessToken()
            {
                Token = token,
                ExpiresIn = expiresMinutes * 60,
            },
            RefreshToken = refreshToken,
        };
    }
    
    /// <summary>
    /// RefreshToken
    /// </summary>
    /// <param name="jwtOption">jwtOption</param>
    /// <param name="refreshToken">expiresMinutes token过期时间</param>
    /// <returns></returns>
    public static TokenResult RefreshToken(ColaJwtOption jwtOption, RefreshToken refreshToken)
    {
                                                              
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var secretKey = jwtOption.Secret;
        var securityKey = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)), SecurityAlgorithms.HmacSha256);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
            }),
            IssuedAt = DateTime.Now,
            NotBefore = DateTime.Now,
            Expires = DateTime.Now.AddMinutes(jwtOption.AccessExpiration),
            SigningCredentials = securityKey
        };
        var securityToken = jwtTokenHandler.CreateToken(tokenDescriptor);
        var token = jwtTokenHandler.WriteToken(securityToken);
        var refreshTokenId = refreshToken.TokenId;
        refreshToken = new RefreshToken()
        {
            JwtId = securityToken.Id,
            TokenId = refreshTokenId,
            CreationTime = DateTime.Now,
            ExpiryTime = DateTime.Now.AddMonths(jwtOption.RefreshExpiration),
            Token = Convert.ToBase64String(Encoding.UTF8.GetBytes(GenerationCodeHelper.GenerationCode(32,true,true,false)))
        };
        return new TokenResult()
        {
            Token = new AccessToken()
            {
                Token = token,
                ExpiresIn = jwtOption.AccessExpiration * 60
            },
            RefreshToken = refreshToken,
        };
    }
}