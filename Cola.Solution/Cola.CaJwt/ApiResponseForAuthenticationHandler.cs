using System.Text.Encodings.Web;
using Cola.Models.Core.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Cola.CaJwt;

public class ApiResponseForAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private IOptionsMonitor<AuthenticationSchemeOptions> _options;
    private IConfiguration _config;
    public ApiResponseForAuthenticationHandler(IConfiguration config, IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
    {
        _options = options;
        _config = config;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        throw new NotImplementedException();
    }
    
    protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.ContentType = "application/json";    
        var apiResult = new ApiResult();
        apiResult.Code = 401;
        apiResult.Data = 401;
        apiResult.Message = "很抱歉，您无权访问该接口，请确保已经登录!";
        await Response.WriteAsync(JsonConvert.SerializeObject(apiResult));
    }
 
    protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
    {
        Response.ContentType = "application/json";        
        var apiResult = new ApiResult();
        apiResult.Code = 403;
        apiResult.Message = "很抱歉，您的访问权限等级不够，联系管理员!!";
        await Response.WriteAsync(JsonConvert.SerializeObject(apiResult));
 
    }
    
    
}