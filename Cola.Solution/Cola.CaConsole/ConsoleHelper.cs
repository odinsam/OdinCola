using Newtonsoft.Json;

namespace Cola.CaConsole;

public class ConsoleHelper
{
    private static string TimestampForma = "[yyyy-MM-dd HH:mm:ss] ";
    
    /// <summary>
    ///  BackgroundColor:Green  ForegroundColor: White
    /// </summary> 
    /// <param name="str"></param>
    public static void WriteInfo(Object str)
    {
        var dt = DateTime.Now.ToString(TimestampForma);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(dt + str);
        Console.ResetColor();
    }
    
    public static void WriteInfo(int str)
    {
        var dt = DateTime.Now.ToString(TimestampForma);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(dt + str);
        Console.ResetColor();
    }
    
    public static void WriteInfo(string str)
    {
        var dt = DateTime.Now.ToString(TimestampForma);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(dt + str);
        Console.ResetColor();
    }

    public static void WriteInfo(string format, params object?[]? arg)
    {
        var dt = DateTime.Now.ToString(TimestampForma);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(dt + format, arg);
        Console.ResetColor();
    }

    /// <summary>
    /// BackgroundColor:DarkRed  ForegroundColor: White
    /// </summary>
    /// <param name="ex"></param>
    public static void WriteException(Exception ex)
    {
        var dt = DateTime.Now.ToString(TimestampForma);
        Console.BackgroundColor = ConsoleColor.DarkRed;
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(dt + JsonConvert.SerializeObject(ex));
        Console.ResetColor();
    }

    public static void WriteException(string format)
    {
        var dt = DateTime.Now.ToString(TimestampForma);
        Console.BackgroundColor = ConsoleColor.DarkRed;
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(dt + format);
        Console.ResetColor();
    }

    public static void WriteException(string format, params object?[]? arg)
    {
        var dt = DateTime.Now.ToString(TimestampForma);
        Console.BackgroundColor = ConsoleColor.DarkRed;
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(dt + format, arg);
        Console.ResetColor();
    }

    /// <summary>
    /// foregroundColor:White   backgroundColor:Black
    /// </summary>
    /// <param name="format"></param>
    /// <param name="foregroundColor"></param>
    /// <param name="backgroundColor"></param>
    /// <param name="arg"></param>
    public static void WriteLine(string format, ConsoleColor foregroundColor = ConsoleColor.White,
        ConsoleColor backgroundColor = ConsoleColor.Black, params object?[]? arg)
    {
        var dt = DateTime.Now.ToString(TimestampForma);
        Console.ForegroundColor = foregroundColor;
        Console.BackgroundColor = backgroundColor;
        Console.WriteLine(dt + format, arg);
        Console.ResetColor();
    }

    public static void WriteLine(string format, ConsoleColor foregroundColor = ConsoleColor.White,
        ConsoleColor backgroundColor = ConsoleColor.Black)
    {
        var dt = DateTime.Now.ToString(TimestampForma);
        Console.ForegroundColor = foregroundColor;
        Console.BackgroundColor = backgroundColor;
        Console.WriteLine(dt + format);
        Console.ResetColor();
    }
}