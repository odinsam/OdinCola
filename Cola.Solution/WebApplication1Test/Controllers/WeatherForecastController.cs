using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Cola.CaCache.IColaCache;
using Cola.CaJwt;
using Cola.CaUtils.Constants;
using Cola.CaUtils.Extensions;
using Cola.CaUtils.Helper;
using Cola.Models.Core.Models;
using Cola.Models.Core.Models.CaJwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NetTaste;

namespace WebApplication1Test.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IColaHybridCache _colaHybridCache;
    private readonly ColaJwtOption _jwtOption;
    private static readonly string[] Summaries =
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    

    public WeatherForecastController(IColaHybridCache colaHybridCache, ILogger<WeatherForecastController> logger,IConfiguration configuration)
    {
        _configuration = configuration;
        _colaHybridCache = colaHybridCache;
        _jwtOption = configuration.GetSection(SystemConstant.CONSTANT_COLAJWT_SECTION).Get<ColaJwtOption>();
    }

    [HttpGet("Test")]
    public string Test()
    {
        return $"test {DateTime.Now:yyyy MMMM dd}";
    }

    [HttpGet("GetToken")]
    public ApiResult  GetToken(string loginName)
    {
        var tokenResult = TokenHelper.CreateToken(
            loginName,
            _jwtOption.Secret,
            _jwtOption.AccessExpiration,
            _jwtOption.RefreshExpiration);
        _colaHybridCache.Set(tokenResult.RefreshToken.TokenId,tokenResult, TimeSpan.FromMinutes(_jwtOption.AccessExpiration));
        return new ApiResult
        {
            Code = 0,
            Token = tokenResult.Token
        };
    }

[HttpGet("ValidateToken")]
public ApiResult ValidateToken(string token,string refreshToken)
{
    try
    {
        var jwtConfig = _configuration.GetSection("ColaJwt");
        var secretKey = _jwtOption.Secret;
        var storedRefreshToken = _colaHybridCache.Get<RefreshToken>(token.ToMd5Lower());
        var validateResult = TokenHelper.ValidateToken(_jwtOption, token, storedRefreshToken);
        storedRefreshToken!.Used = validateResult;
        var isRenewal = storedRefreshToken.IsRenewal(jwtConfig.GetValue<int>("RenewalExpir"));
        return new ApiResult()
        {
            Message = "validate success, refreshToken is used"
        };
    }
    catch
    {
        return null;
    }
}
    
    [Authorize]
    [HttpGet("GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
    

    // [Idempotent]
    // [EnableCors("cors")]
    // [HttpPost(Name = "PostWeatherForecast")]
    // public Student Post()
    // {
    //     return new Student
    //     {
    //         StdId = 1,
    //         StdName = "odinsam"
    //     };
    // }
}

public class Student
{
    public int StdId { get; set; }
    public string? StdName { get; set; }
}