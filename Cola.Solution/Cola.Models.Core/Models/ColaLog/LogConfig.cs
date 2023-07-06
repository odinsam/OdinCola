using SqlSugar;

namespace Cola.Models.Core.Models.ColaLog;

public class LogConfig
{
    /// <summary>
    ///     Log 时间格式 默认: yyyy-MM-dd HH:mm:ss
    /// </summary>
    public string LogTimeFormat { get; set; } = "yyyy-MM-dd HH:mm:ss";

    /// <summary>
    ///     Log 分隔符 默认 *
    /// </summary>
    public string? LogSeparator { get; set; } = "*";

    public string? SaveType { get; set; } = "Print";

    public DbType DataBaseType { get; set; } = DbType.MySql;

    /// <summary>
    ///     连接字符串
    /// </summary>
    public string? DbConnectionString { get; set; } = "";

    public string DirectoryName { get; set; } = "SystemLog";

    public int KeepTime { get; set; } = 30;
}