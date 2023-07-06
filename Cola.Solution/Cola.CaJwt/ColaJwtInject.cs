using System.Text;
using Cola.CaConsole;
using Cola.CaUtils.Constants;
using Cola.Models.Core.Models.CaJwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Cola.CaJwt;

public static class ColaJwtInject
{
    
    /// <summary>
    /// AddColaJwt
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static AuthenticationBuilder AddColaJwt<T>(
        this IServiceCollection services,
        IConfiguration config) where T : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        ConsoleHelper.WriteInfo("注入【 Custom Scheme AuthenticationHandler 】");
        return services.AddColaJwtAuthentication<T>().AddColaJwtBearer(config)
            .AddColaJwtScheme<T>();
    }
    
    /// <summary>
    /// AddColaJwt
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static AuthenticationBuilder AddColaJwt(
        this IServiceCollection services,
        IConfiguration config)
    {
        ConsoleHelper.WriteInfo("注入【 ApiResponseForAuthenticationHandler 】");
        return services.AddColaJwtAuthentication<ApiResponseForAuthenticationHandler>().AddColaJwtBearer(config)
                    .AddColaJwtScheme<ApiResponseForAuthenticationHandler>();
    }
    
    /// <summary>
    /// AddColaJwtWithDefaultScheme
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static AuthenticationBuilder AddColaJwtWithDefaultScheme(
        this IServiceCollection services,
        IConfiguration config)
    {
        ConsoleHelper.WriteInfo("注入【 AddColaJwtWithDefaultScheme 】");
        return services.AddColaJwtDefaultAuthentication().AddColaJwtBearer(config);
    }

    /// <summary>
    /// AddColaJwtAuthentication
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static AuthenticationBuilder AddColaJwtAuthentication<T>(
        this IServiceCollection services)
    {
        var injectResult = services
            .AddAuthentication(x =>
            {
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = nameof(T);
                x.DefaultForbidScheme = nameof(T);
            });
        ConsoleHelper.WriteInfo("注入【 ColaJwtAuthentication 】");
        return injectResult;
    }
    
    /// <summary>
    /// AddColaJwtAuthentication
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static AuthenticationBuilder AddColaJwtDefaultAuthentication(
        this IServiceCollection services)
    {
        var injectResult = services
            .AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
            });
        ConsoleHelper.WriteInfo("注入【 ColaJwtAuthentication 】");
        return injectResult;
    }

    /// <summary>
    /// AddColaJwtBearer
    /// </summary>
    /// <param name="authenticationBuilder"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static AuthenticationBuilder AddColaJwtBearer(
        this  AuthenticationBuilder authenticationBuilder,
        IConfiguration config)
    {
        var jwtConfig = config.GetSection(SystemConstant.CONSTANT_COLAJWT_SECTION).Get<ColaJwtOption>();
        jwtConfig = jwtConfig ?? new ColaJwtOption();
        authenticationBuilder
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters()
                {
                    //是否验证SecurityKey
                    ValidateIssuerSigningKey = true,
                    //设置SecurityKey
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secret)),
                    //发行人 //Issuer，这两项和前面签发jwt的设置一致
                    ValidIssuer = jwtConfig.IsUser,
                    //订阅人
                    ValidAudience = jwtConfig.Audience,
                    //是否验证Issuer
                    ValidateIssuer = false,
                    //是否验证Audience
                    ValidateAudience = false,
                    //是否验证失效时间
                    ValidateLifetime = true,
                    //这个是缓冲过期时间，也就是说，即使我们配置了过期时间，这里也要考虑进去，过期时间+缓冲，默认好像是7分钟，你可以直接设置为0
                    ClockSkew = TimeSpan.Zero,
                };
            });
        ConsoleHelper.WriteInfo("注入【 ColaJwtBearer 】");
        return authenticationBuilder;
    }

    /// <summary>
    /// AddColaJwtBearer
    /// </summary>
    /// <param name="authenticationBuilder"></param>
    /// <returns></returns>
    public static AuthenticationBuilder AddColaJwtScheme<T>(
        this AuthenticationBuilder authenticationBuilder) where T : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        authenticationBuilder.AddScheme<AuthenticationSchemeOptions,T>(
            nameof(T), o=>
            {
                    
            });
        ConsoleHelper.WriteInfo("注入【 ColaJwtScheme 】");
        return authenticationBuilder;
    }

    /// <summary>
    /// AddColaJwt
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddColaSwaggerGen(
        this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            var scheme = new OpenApiSecurityScheme()
            {
                Description = "Authorization header. \r\nExample: 'Bearer 123456'",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Authorization"
                },
                Scheme = "oauth2",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
            };
            c.AddSecurityDefinition("Authorization", scheme);
            var requirement = new OpenApiSecurityRequirement();
            requirement[scheme] = new List<string>();
            c.AddSecurityRequirement(requirement);
        });
        return services;
    }
}