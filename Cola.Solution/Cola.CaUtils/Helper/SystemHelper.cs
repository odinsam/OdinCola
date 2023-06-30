using System.Reflection;

namespace Cola.CaUtils.Helper;

public class SystemHelper
{
    /// <summary>
    /// 获取当前Net Core的版本
    /// </summary>
    public static Version? NetCoreVersion => typeof(object).GetTypeInfo().Assembly.GetName().Version;
    
    
    /// <summary>
    /// IsWindows
    /// </summary>
    public  static bool IsWindows => OperatingSystem.IsWindows();
    
    /// <summary>
    /// IsLinux
    /// </summary>
    public  static bool IsLinux => OperatingSystem.IsLinux();
    
    /// <summary>
    /// IsMacOS
    /// </summary>
    public  static bool IsMacOS => OperatingSystem.IsMacOS();
    
    /// <summary>
    /// IsLinuxOrMacOS
    /// </summary>
    public  static bool IsLinuxOrMacOS => OperatingSystem.IsLinux() || OperatingSystem.IsMacOS();
}