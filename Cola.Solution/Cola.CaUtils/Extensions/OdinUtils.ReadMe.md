### **OdinUtils.ReadMe 封装说明**

#### 1. 枚举操作

```csharp
// 获取枚举描述
string GetDescription(this Enum enumValue)
// 转换枚举类型
T ParseEnum<T>(this object oValue)
```

#### 2. 异常封装

#### 3. 文件操作封装

#### 4. HttpRequest封装

```csharp
/// <summary>
/// 获取 QueryString
/// </summary>
/// <param name="requestUri">请求的uri</param>
/// <returns>QueryString key-value 形式</returns>
public static Dictionary<string, string> GetRequestQueryString(this string requestUri)

/// <summary>
/// 获取请求对象内容
/// </summary>
/// <param name="request">request 请求对象</param>
/// <returns></returns>
public static string ReadRequestBody(this HttpRequest request)
```

#### 5. Number封装

#### 6. Object封装

```csharp
/// <summary>
/// 转换 对象 为 Dictionary&lt;string, string&gt;类型
/// </summary>
/// <param name="obj">泛型对象</param>
/// <param name="encoder">字符编码</param>
/// <typeparam name="T">泛型类型</typeparam>
/// <returns>Dictionary&lt;string, string&gt;类型</returns>
public static Dictionary<string, string> ConvertObjectToDictionary<T>(this T obj, Encoding encoder = null) where T : class

/// <summary>
/// 对象转 JsonString
/// </summary>
/// <param name="obj">需要转换的对象</param>
/// <param name="stringFormat">是否需要json格式化输出</param>
/// <returns>转换后的string</returns>
public static string ToJson(this Object obj, EnumStringFormat stringFormat = EnumStringFormat.None)

/// <summary>
/// 可以处理复杂映射
/// </summary>
/// <typeparam name="TIn">输入类</typeparam>
/// <typeparam name="TOut">输出类</typeparam>
/// <param name="expression">表达式目录树,可以为null</param>
/// <param name="tIn">输入实例</param>
/// <returns></returns>
public static TOut Mapper<TIn, TOut>(this TIn tIn, Expression<Func<TIn, TOut>> expression = null)


```

#### 7. String封装

```csharp
/// <summary>
/// Indicates whether the specified string is null or an empty string ("").
/// </summary>
/// <param name="str">The string to test</param>
/// <returns>true if the value parameter is null or an empty string (""); otherwise, false.</returns>
public static bool IsNullOrEmpty(this string str)

/// <summary>  
/// Unicode字符串转为正常字符串  
/// </summary>  
/// <param name="srcText"></param>  
/// <returns></returns>  
public static string UnicodeToString(this string srcText)

/// <summary>
/// string to ascii
/// </summary>
/// <param name="str">string</param>
/// <exception cref="ArgumentNullException"></exception>
/// <returns>ascii code</returns>
public static string ToAscii(this string str)

/// <summary>
/// StringToBase64String
/// </summary>
/// <param name="str">string</param>
/// <returns>Base64String</returns>
public static string StringToBase64String(this string str)

/// <summary>
/// Base64StringToString
/// </summary>
/// <param name="str">Base64String</param>
/// <returns>String</returns>
public static string Base64StringToString(this string str)

/// <summary>
/// 格式化json字符串
/// </summary>
/// <param name="str">json string</param>
/// <returns>json format string</returns>
public static string ToJsonFormatString(this string str)

/// <summary>
/// to xml string
/// </summary>
/// <param name="xmLString">xml string</param>
/// <returns>xml format string</returns>
public static string ToXmlFormatString(this string xmLString)

/// <summary>
/// string转16进制
/// </summary>
/// <param name="strAscii">string to ascii</param>
/// <param name="separator">分隔符</param>
/// <returns>16进制的string</returns>
public static string ConvertStringToHex(this string strAscii, string separator = null)

/// <summary>
/// 16进制转string
/// </summary>
/// <param name="hexValue">16进制string</param>
/// <param name="separator">分隔符</param>
/// <returns>string说</returns>
public static string ConvertHexToString(this string hexValue, string separator = null)

/// <summary>
/// 将指定的16进制字符串转换为byte数组
/// </summary>
/// <param name="s">16进制字符串(如：“7F 2C 4A”或“7F2C4A”都可以)</param>
/// <returns>16进制字符串对应的byte数组</returns>
public static byte[] ConvertHexStringToByteArray(this string s)

/// <summary>
/// string to bytes
/// </summary>
/// <param name="str">string</param>
/// <returns>byte array</returns>
public static byte[] GetBytesFromString(this string str)

/// <summary>
/// bytes to string
/// </summary>
/// <param name="bytes">byte array</param>
/// <returns>string</returns>
public static string ConvertStringToBytes(this byte[] bytes)

/// <summary>
/// Utf8ToGb2312
/// </summary>
/// <param name="text">string</param>
/// <returns>Gb2312 string</returns>
public static string Utf8ToGb2312(this string text)

/// <summary>
/// StringEncode
/// </summary>
/// <param name="text">string</param>
/// <param name="sourceEncode">source encode</param>
/// <param name="transEncode">trans Encode</param>
/// <returns>string</returns>
public static string ConvertStringEncode(this string text, Encoding sourceEncode, Encoding transEncode)

/// <summary>
/// string to Enum
/// </summary>
/// <param name="text"></param>
/// <typeparam name="T"></typeparam>
/// <returns></returns>
public static T ConvertStringToEnum<T>(this string text)

/// <summary>
/// QueryStringToJson
/// </summary>
/// <param name="queryString">http request queryString</param>
/// <returns>json string</returns>
public static string QueryStringToJson(this string queryString)

/// <summary>
/// ConvertStringToDateTime
/// </summary>
/// <param name="dateTimeStr">dateTimeStr</param>
/// <param name="dateTimeFormat">dateTimeFormat</param>
/// <returns>DateTime</returns>
public static DateTime ConvertStringToDateTime(this string dateTimeStr, string dateTimeFormat)
```

#### 9. String正则

```csharp
/// <summary>
/// 判断中国身份号码格式
/// </summary>
/// <param name="cardId">cardId</param>
/// <param name="pattern">pattern</param>
/// <returns>true if the value parameter is cardId; otherwise, false.</returns>
public static bool IsChinaCardId(this string cardId, string pattern = null)

/// <summary>
/// 判断中国移动电话号码格式
/// </summary>
/// <param name="mobile">mobile</param>
/// <param name="pattern">pattern</param>
/// <returns>true if the value parameter is mobile; otherwise, false.</returns>
public static bool IsChinaMobile(this string mobile, string pattern = null)

/// <summary>
/// 判断Ip地址格式
/// </summary>
/// <param name="ipAddress">ipAddress</param>
/// <param name="pattern">pattern</param>
/// <returns>true if the value parameter is ipAddress; otherwise, false.</returns>
public static bool IsIpAddress(this string ipAddress, string pattern = null)

/// <summary>
/// 判断邮箱地址格式
/// </summary>
/// <param name="email">邮箱地址</param>
/// <param name="pattern">pattern</param>
/// <returns>true if the value parameter is email; otherwise, false.</returns>
public static bool IsEmail(this string email, string pattern = null)

/// <summary>
/// 判断uri格式
/// </summary>
/// <param name="uri">uri</param>
/// <param name="pattern"></param>
/// <returns>true if the value parameter is uri; otherwise, false.</returns>
public static bool IsUri(this string uri, string pattern = null)

/// <summary>
/// 判断ip4格式
/// </summary>
/// <param name="ip">ip</param>
/// <param name="pattern">pattern</param>
/// <returns></returns>
public static string IsIp4(this string ip, string pattern = null)
```

#### 10. Byte封装

```csharp
/// <summary>
/// bytes to string
/// </summary>
/// <param name="bytes">byte array</param>
/// <returns>string</returns>
public static string GetStringFromBytes(this byte[] bytes)

/// <summary>
/// 将一个byte数组转换成一个格式化的16进制字符串
/// </summary>
/// <param name="data">byte数组</param>
/// <returns>格式化的16进制字符串</returns>
public static string ConvertByteArrayToHexString(this byte[] data)
```

#### 11. DateTime封装

```csharp
/// <summary>
/// ToUnixTime时间戳
/// </summary>
/// <param name="now">DateTime now</param>
/// <returns>时间戳</returns>
public static long ToUnixTime(this DateTime now)

/// <summary>
/// ToUnixTime时间戳 (毫秒级别)
/// </summary>
/// <param name="now">DateTime now</param>
/// <returns>时间戳</returns>
public static long ToUnixTimeMs(this DateTime now)

/// <summary>
/// 时间戳反转为时间，有很多中翻转方法，但是，请不要使用过字符串（string）进行操作，大家都知道字符串会很慢！
/// </summary>
/// <param name="timeStamp">时间戳</param>
/// <param name="ssOrMs">是否精确到毫秒</param>
/// <returns>返回一个日期时间</returns>
public static DateTime ToTimer(this long timeStamp, EnumSsOrMs? ssOrMs = EnumSsOrMs.Ss)

/// <summary>
/// 时间差返回 MM:dd HH:mm
/// </summary>
/// <returns></returns>
public static string TimeDifferenceToString(this long longTs, long dtNow)
```

#### 12. JObject封装

```csharp
/// <summary>
/// JArray To JObjectList
/// </summary>
/// <param name="jary"></param>
/// <typeparam name="T"></typeparam>
/// <returns></returns>
public static JObjectList<T> ConvertJArrayToJObjectList<T>(this JArray jary)

/// <summary>
/// ConvertJTokenToJObjectList
/// </summary>
/// <param name="jtoken"></param>
/// <typeparam name="T"></typeparam>
/// <returns></returns>
public static JObjectList<T> ConvertJTokenToJObjectList<T>(this JToken jtoken)

/// <summary>
/// JObject add key/value
/// </summary>
/// <param name="jObj"></param>
/// <param name="key"></param>
/// <param name="value"></param>
/// <typeparam name="T"></typeparam>
public static void Add<T>(this JObject jObj, string key, JObjectList<T> value)
```

#### 13. TypeAdapter 映射封装

```csharp
/// <summary>
/// 集合类型对象转换类型映射
/// </summary>
/// <param name="source">需要转换的源对象</param>
/// <typeparam name="TSource">源类型</typeparam>
/// <typeparam name="TDestination">目标类型</typeparam>
/// <typeparam name="T">最终映射转换后的类型</typeparam>
/// <returns>通过映射规则转换后的对象, 具体使用请参看 readme</returns>
public static T OdinCollectionAdapter<TSource, TDestination, T>(this Object source)

/// <summary>
/// 集合类型对象转换类型映射
/// </summary>
/// <param name="source">需要转换的源对象</param>
/// <param name="options">自定义转换规则</param>
/// <typeparam name="TSource">源类型</typeparam>
/// <typeparam name="TDestination">目标类型</typeparam>
/// <typeparam name="T">最终映射转换后的类型</typeparam>
/// <returns>通过映射规则转换后的对象, 具体使用请参看 readme</returns>
public static T CollectionAdapter<TSource, TDestination, T>(
    this Object source,
    Action<TypeAdapterSetter<TSource, TDestination>>? options
)

/// <summary>
/// 集合类型对象转换类型映射
/// </summary>
/// <param name="source">需要转换的源对象</param>
/// <param name="config">全局映射规则</param>
/// <typeparam name="TSource">源类型</typeparam>
/// <typeparam name="TDestination">目标类型</typeparam>
/// <typeparam name="T">最终映射转换后的类型</typeparam>
/// <returns>通过映射规则转换后的对象, 具体使用请参看 readme</returns>
public static T CollectionAdapter<TSource, TDestination, T>(
    this Object source,
    TypeAdapterConfig? config
)

/// <summary>
/// 集合类型对象转换类型映射
/// </summary>
/// <param name="source">需要转换的源对象</param>
/// <param name="options">自定义转换规则</param>
/// <param name="config">全局映射规则</param>
/// <typeparam name="TSource">源类型</typeparam>
/// <typeparam name="TDestination">目标类型</typeparam>
/// <typeparam name="T">最终映射转换后的类型</typeparam>
/// <returns>通过映射规则转换后的对象, 具体使用请参看 readme</returns>
public static T CollectionAdapter<TSource, TDestination, T>(
    this Object source,
    Action<TypeAdapterSetter<TSource, TDestination>> options,
    TypeAdapterConfig config
)

/// <summary>
/// 对象转换类型映射
/// </summary>
/// <param name="source">需要转换的源对象</param>
/// <typeparam name="TSource">源类型</typeparam>
/// <typeparam name="TDestination">目标类型</typeparam>
/// <returns>通过映射规则转换后的对象, 具体使用请参看 readme</returns>
public static TDestination Adapter<TSource, TDestination>(this Object source)

/// <summary>
/// 对象转换类型映射
/// </summary>
/// <param name="source">需要转换的源对象</param>
/// <param name="options">自定义转换规则</param>
/// <typeparam name="TSource">源类型</typeparam>
/// <typeparam name="TDestination">目标类型</typeparam>
/// <returns>通过映射规则转换后的对象, 具体使用请参看 readme</returns>
public static TDestination Adapter<TSource, TDestination>(
    this TSource source,
    Action<TypeAdapterSetter<TSource, TDestination>> options
)

/// <summary>
/// 对象转换类型映射
/// </summary>
/// <param name="source">需要转换的源对象</param>
/// <param name="config">全局映射规则</param>
/// <typeparam name="TSource">源类型</typeparam>
/// <typeparam name="TDestination">目标类型</typeparam>
/// <returns>通过映射规则转换后的对象, 具体使用请参看 readme</returns>
public static TDestination Adapter<TSource, TDestination>(
    this TSource source,
    TypeAdapterConfig config
)

/// <summary>
/// 对象转换类型映射
/// </summary>
/// <param name="source">需要转换的源对象</param>
/// <param name="options">自定义转换规则</param>
/// <param name="config">全局映射规则</param>
/// <typeparam name="TSource">源类型</typeparam>
/// <typeparam name="TDestination">目标类型</typeparam>
/// <returns>通过映射规则转换后的对象, 具体使用请参看 readme</returns>
public static TDestination Adapter<TSource, TDestination>(
    this TSource source,
    Action<TypeAdapterSetter<TSource, TDestination>> options,
    TypeAdapterConfig config
)
```













