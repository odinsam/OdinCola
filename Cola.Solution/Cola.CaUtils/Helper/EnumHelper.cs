using System.Reflection;
using System.Reflection.Emit;

namespace Cola.CaUtils.Helper;

/// <summary>
///     EnumHelper
/// </summary>
public class EnumHelper
{
    /// <summary>
    ///     动态创建枚举
    /// </summary>
    /// <param name="enumDictionary">枚举元素列表</param>
    /// <param name="enumName">枚举名</param>
    /// <returns>Enum枚举</returns>
    public static Type? CreateEnum(Dictionary<string, int>? enumDictionary, string enumName = "DefalutEnum")
    {
        if (enumDictionary == null || enumDictionary.Count <= 0)
            return null;

        var currentDomain = AppDomain.CurrentDomain;
        var aName = new AssemblyName("OdinDynamicAssembly");
        var ab = AssemblyBuilder.DefineDynamicAssembly(aName, AssemblyBuilderAccess.Run);
        var mb = ab.DefineDynamicModule(aName.Name!);
        var eb = mb.DefineEnum("Elevation", TypeAttributes.Public, typeof(int));
        foreach (var item in enumDictionary) eb.DefineLiteral(item.Key, item.Value);

        Type? finished = eb.CreateTypeInfo();
        return finished;
    }

    /// <summary>
    ///     动态创建枚举
    ///     <code>
    /// <para>foreach (object o in Enum.GetValues(finished))</para> 
    /// <para>{</para> 
    /// Console.WriteLine("{0}.{1} = {2}", finished, o, ((int) o));
    /// <para>}</para> 
    /// </code>
    /// </summary>
    /// <param name="enumList"></param>
    /// <param name="enumName">枚举名</param>
    /// <returns> Enum枚举</returns>
    public static Type? CreateEnum(List<string>? enumList, string enumName = "DefalutEnum")
    {
        if (enumList == null || enumList.Count <= 0)
            return null;
        var currentDomain = AppDomain.CurrentDomain;
        var aName = new AssemblyName("OdinDynamicAssembly");
        var ab = AssemblyBuilder.DefineDynamicAssembly(aName, AssemblyBuilderAccess.Run);
        var mb = ab.DefineDynamicModule(aName.Name!);
        var eb = mb.DefineEnum("Elevation", TypeAttributes.Public, typeof(int));
        for (var i = 0; i < enumList.Count; i++) eb.DefineLiteral(enumList[i], i);

        Type? finished = eb.CreateTypeInfo();
        return finished;
    }
}