### Cola.CaException 异常类
#### 1. 接口说明
```csharp
/// <summary>
/// throw number>0
/// </summary>
/// <param name="i"></param>
/// <param name="errorMessage"></param>
/// <returns></returns>
Exception? ThrowGreaterThanZero(int i, string errorMessage);

/// <summary>
/// object is null
/// </summary>
/// <param name="obj"></param>
/// <typeparam name="T"></typeparam>
/// <returns></returns>
Exception? ThrowIfNull<T>(T obj);

/// <summary>
/// throw String Is NullOrEmpty
/// </summary>
/// <param name="str"></param>
/// <param name="exMessage"></param>
/// <returns></returns>
Exception? ThrowStringIsNullOrEmpty(string str, string exMessage);

/// <summary>
/// Throw Exception
/// </summary>
/// <param name="str"></param>
/// <returns></returns>
Exception ThrowException(string str);

/// <summary>
/// Throw Exception
/// </summary>
/// <param name="ex"></param>
/// <returns></returns>
Exception ThrowException(Exception ex);
```