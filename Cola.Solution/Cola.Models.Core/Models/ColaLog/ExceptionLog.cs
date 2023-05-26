namespace Cola.Models.Core.Models.ColaLog;

public class ExceptionLog : LogInfo
{
    public Exception? LogException { get; set; }
}