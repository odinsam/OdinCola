using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Cola.CaCache.IColaCache;
using Cola.CaJwt;
using Cola.CaUtils.Constants;
using Cola.CaUtils.Extensions;
using Cola.CaUtils.Helper;
using Cola.Models.Core.Models.CaJwt;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace WebApplication1Test;

public class ColaAsyncAuthorizationFilter : IAsyncAuthorizationFilter
{
    private readonly IConfiguration _configuration;
    private readonly IColaHybridCache _colaHybridCache;
    private readonly ColaJwtOption _jwtOption;

    public ColaAsyncAuthorizationFilter(IColaHybridCache colaHybridCache,IConfiguration configuration)
    {
        _configuration = configuration;
        _colaHybridCache = colaHybridCache;
        _jwtOption = configuration.GetSection(SystemConstant.CONSTANT_COLAJWT_SECTION).Get<ColaJwtOption>();
    }
    public Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var action = context.ActionDescriptor as ControllerActionDescriptor;
        var identity = context.HttpContext.User.Identity;
        if (identity != null)
        {
            var loginName = identity.Name;
            var tokenResult = _colaHybridCache.Get<TokenResult>(loginName!.ToMd5Lower());
            var validateResult = TokenHelper.ValidateToken(_jwtOption, tokenResult.Token.Token, tokenResult.RefreshToken);
            if (validateResult)
            {
                if (tokenResult.RefreshToken.IsRenewal(_jwtOption.RenewalExpir))
                {
                    
                }
            }
        }
        // context.HttpContext.User.Identity.Name
        // var name = GetClaimValue(HttpContext,"unique_name");
        // var permission = action.MethodInfo.GetCustomAttribute<PermissionAttribute>();
        Console.WriteLine("OnAuthorizationAsync");
        return Task.CompletedTask;
    }
    
    /// <summary>
    /// RefreshToken
    /// </summary>
    /// <param name="loginName">loginName</param>
    /// <param name="refreshToken">refreshToken</param>
    /// <returns></returns>
    public TokenResult RefreshToken(string loginName, RefreshToken refreshToken)
    {
        var expiresMinutes = _jwtOption.AccessExpiration;
        var refreshTokenExpiresMinutes = _jwtOption.RefreshExpiration;
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var securityKey = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOption.Secret)), SecurityAlgorithms.HmacSha256);
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
        refreshToken = new RefreshToken()
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
                ExpiresIn = _jwtOption.AccessExpiration
            },
            RefreshToken = refreshToken,
        };
    }
}