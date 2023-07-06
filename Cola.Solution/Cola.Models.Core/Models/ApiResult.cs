using Cola.CaUtils;
using Cola.CaUtils.Enums;
using Cola.Models.Core.Models.CaJwt;

namespace Cola.Models.Core.Models;

/// <summary>
///     方法返回类型
/// </summary>
public class ApiResult
{
    /// <summary>
    ///     方法返回结果标识
    /// </summary>
    private int _code;

    public ApiResult(object? data = null, string? message = null,
        EnumResult enumResult = EnumResult.Success,
        Exception? error = null)
    {
        Data = data;
        Message = message;
        Code = (int)enumResult;
        Error = error;
    }

    /// <summary>
    ///     方法返回数据
    /// </summary>
    public object? Data { get; set; }

    /// <summary>
    ///     方法返回信息
    /// </summary>
    public string? Message { get; set; }

    public int Code
    {
        get => _code;
        set
        {
            try
            {
                var result = (EnumResult)value;
                _code = value;
            }
            catch
            {
                throw new ColaException(EnumException.ParamOutOfRang);
            }
        }
    }

    /// <summary>
    ///     返回异常对象
    /// </summary>
    public Exception? Error { get; set; }

    public AccessToken Token { get; set; }
}