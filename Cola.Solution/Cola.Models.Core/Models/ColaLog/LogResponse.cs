using Cola.Models.Core.Enums.Logs;

namespace Cola.Models.Core.Models.ColaLog;

/// <summary>
///     LogResponse
/// </summary>
public class LogResponse
{
    public string? LogId { get; set; }
    public EnumLogLevel LogLevel { get; set; }
}