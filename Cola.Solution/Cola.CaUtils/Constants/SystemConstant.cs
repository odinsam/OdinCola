namespace Cola.CaUtils.Constants;

/// <summary>
///     系统常量
/// </summary>
public class SystemConstant
{
    public const string CONSTANT_SERVER_SECTION = "Server";

    #region OpenApi 常量

    public const string CONSTANT_HEALTH_CHECK_PATH = "/health";

    #endregion

    #region 基础常量

    /// <summary>
    ///     CONSTANT_HTTPS
    /// </summary>
    public const string CONSTANT_HTTPS = "https";

    /// <summary>
    ///     CONSTANT_HTTP
    /// </summary>
    public const string CONSTANT_HTTP = "http";

    #endregion

    #region HystrixCommand Polly 常量

    public const int CONSTANT_MAXRETRYCOUNT_HYSTRIXCOMMAND = 0;
    public const int CONSTANT_RETRYINTERVALMILLISECONDS_HYSTRIXCOMMAND = 500;
    public const bool CONSTANT_ENABLECIRCUITBREAKER_HYSTRIXCOMMAND = false;
    public const int CONSTANT_EXCEPTIONSALLOWEDBEFOREBREAKING_HYSTRIXCOMMAND = 0;
    public const int CONSTANT_MILLISECONDSOFBREAK_HYSTRIXCOMMAND = 0;
    public const int CONSTANT_TIMEOUTMILLISECONDS_HYSTRIXCOMMAND = 0;
    public const int CONSTANT_CACHETTLMILLISECOUNDS_HYSTRIXCOMMAND = 0;

    #endregion

    #region Security

    public const string CONSTANT_SECURITY_SECTION = "Security";
    public const string CONSTANT_SECURITY_RSA_PUBLICKEY_SECTION = "Security:RSA:PublicKey";
    public const string CONSTANT_SECURITY_RSA_PUBLICKEY_FILEPATH_SECTION = "Security/rsa/rsa_public_key.crt";
    public const string CONSTANT_SECURITY_RSA_PRIVATEKEY_FILEPATH_SECTION = "Security/rsa/rsa_private_key.pem";

    #endregion

    #region Config配置常量

    #region 雪花ID配置

    public const string CONSTANT_COLASNOWFLAKE_SECTION = "ColaSnowFlake";

    #endregion

    #region WebApi配置

    public const string CONSTANT_COLAWEBAPI_SECTION = "ColaWebApi";
    public const string CONSTANT_COLAWEBAPI_DEFAULTSETTINGS_SECTION = "ColaWebApiSettings";
    public const string CONSTANT_COLAWEBAPI_FORBIDDENCLIENTNAME_SECTION = "default";
    public const string CONSTANT_COLAWEBAPI_DECOMPRESSION_SECTION = "None,GZip,Deflate,Brotli,All";

    #endregion

    #region Log配置

    public const string CONSTANT_COLALOGS_SECTION = "ColaLogs";

    #endregion

    #region 缓存配置

    public const string CONSTANT_COLACACHE_SECTION = "ColaCache";

    #endregion

    #region EF 配置

    public const string CONSTANT_COLAORM_SECTION = "ColaOrm";

    #endregion"

    #endregion
}