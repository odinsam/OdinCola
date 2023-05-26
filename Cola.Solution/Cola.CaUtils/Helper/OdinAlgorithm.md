### **算法操作封装**

#### 1. 随机数相关

```csharp
/// <summary>
/// ~ 随机种子值
/// </summary>
/// <returns></returns> 
public static int GetRandomSeed()

/// <summary>
/// ~  按权重返回对应需要个数的数组
/// </summary>
/// <param name="list">受权重影响的数组</param>
/// <param name="count">需要返回的个数</param>
/// <typeparam name="T">权重对象的类型，唯一标识</typeparam>
/// <returns></returns>
public static IEnumerable<KeyValuePair<T, int>> GetRandomListByWeight<T>(List<KeyValuePair<T, int>> list, int count)
```

#### 2. 字符串相关

```csharp
/// <summary>
/// 按长度生成对应的字符串
/// </summary>
/// <param name="length">长度</param>
/// <param name="containNum">是否包含数字</param>
/// <param name="containLetter">是否包含字母</param>
/// <param name="containSpecial">是否包含特殊字符</param>
/// <returns>生成的字符串</returns>
public static string GenerationCode(int length, bool containNum = true, bool containLetter = true, bool containSpecial = true)

/// <summary>
/// 不包含 0 o 1 I 等容易混淆的之母和数字
/// </summary>
/// <param name="length">长度</param>
/// <param name="containNum">是否包含数据</param>
/// <param name="containLetter">是否包含字母</param>
/// <param name="containSpecial">是否包含特殊字符</param>
/// <returns>生成的字符串</returns>
public static string GenerationlimpidCode(int length, bool containNum = true, bool containLetter = true, bool containSpecial = true)
```