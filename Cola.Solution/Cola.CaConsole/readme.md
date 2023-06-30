### ConsoleHelper 带背景色与字体色控制台输出
#### 1. 接口说明
```csharp
/// <summary>
///  BackgroundColor:Green  ForegroundColor: White
/// </summary> 
/// <param name="str"></param>
public static void WriteInfo(Object str)
public static void WriteInfo(int str)
public static void WriteInfo(string str)
public static void WriteInfo(string format, params object?[]? arg)


/// <summary>
/// BackgroundColor:DarkRed  ForegroundColor: White
/// </summary>
/// <param name="ex"></param>
public static void WriteException(Exception ex)
public static void WriteException(string format)
public static void WriteException(string format, params object?[]? arg)


/// <summary>
/// foregroundColor:White   backgroundColor:Black
/// </summary>
/// <param name="format"></param>
/// <param name="foregroundColor"></param>
/// <param name="backgroundColor"></param>
/// <param name="arg"></param>
public static void WriteLine(string format, ConsoleColor foregroundColor = ConsoleColor.White,
        ConsoleColor backgroundColor = ConsoleColor.Black, params object?[]? arg)
public static void WriteLine(string format, ConsoleColor foregroundColor = ConsoleColor.White,
        ConsoleColor backgroundColor = ConsoleColor.Black)
```
