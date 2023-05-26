using Cola.CaConsole;
using Cola.CaLog.Core;

namespace Cola.CaException;

public class ColaException : IColaException
{
    private readonly IColaLogs? _colaLog;

    public ColaException(IColaLogs log)
    {
        _colaLog = log;
    }

    /// <summary>
    /// throw number>0
    /// </summary>
    /// <param name="i"></param>
    /// <param name="errorMessage"></param>
    /// <returns></returns>
    public Exception? ThrowGreaterThanZero(int i, string errorMessage)
    {
        if (i > 0)
            return ThrowException(errorMessage);
        return null;
    }
    
    /// <summary>
    /// object is null
    /// </summary>
    /// <param name="obj"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public Exception? ThrowIfNull<T>(T obj)
    {
        if (obj == null)
            return ThrowException($"{typeof(T).FullName} 参数不能为空");
        return null;
    }

    /// <summary>
    /// throw String Is NullOrEmpty
    /// </summary>
    /// <param name="str"></param>
    /// <param name="exMessage"></param>
    /// <returns></returns>
    public Exception? ThrowStringIsNullOrEmpty(string str, string exMessage)
    {
        if (string.IsNullOrEmpty(str))
            return ThrowException($"{exMessage} 不能为空");
        return null;
    }

    /// <summary>
    /// Throw Exception
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public Exception ThrowException(string str)
    {
        var ex = new Exception(str);
        if (_colaLog != null)
            _colaLog!.Error(ex);
        else
            ConsoleHelper.WriteException(ex);
        return ex;
    }

    /// <summary>
    /// Throw Exception
    /// </summary>
    /// <param name="ex"></param>
    /// <returns></returns>
    public Exception ThrowException(Exception ex)
    {
        if (_colaLog != null)
            _colaLog!.Error(ex);
        else
            ConsoleHelper.WriteException(ex);
        return ex;
    }
}