using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Cola.CaUtils.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Formatting = Newtonsoft.Json.Formatting;

namespace Cola.CaUtils.Extensions;

/// <summary>
///     StringExtensions
/// </summary>
public static class StringExtensions
{
    /// <summary>
    ///     toInt
    /// </summary>
    /// <param name="str">string</param>
    /// <returns></returns>
    public static int ToInt(this string str)
    {
        return Convert.ToInt32(str);
    }

    /// <summary>
    ///     compare string ignore case
    /// </summary>
    /// <param name="str">string</param>
    /// <param name="compareString">compare string</param>
    /// <returns></returns>
    public static bool StringCompareIgnoreCase(this string str, string compareString)
    {
        return string.Compare(str, compareString, StringComparison.OrdinalIgnoreCase) == 0;
    }

    /// <summary>
    ///     Indicates whether the specified string is null or an empty string ("").
    /// </summary>
    /// <param name="str">The string to test</param>
    /// <returns>true if the value parameter is null or an empty string (""); otherwise, false.</returns>
    public static bool IsNullOrEmpty(this string? str)
    {
        return string.IsNullOrEmpty(str);
    }

    /// <summary>
    ///     Unicode字符串转为正常字符串
    /// </summary>
    /// <param name="srcText"></param>
    /// <returns></returns>
    public static string UnicodeToString(this string srcText)
    {
        return Regex.Unescape(srcText);
    }

    /// <summary>
    ///     string to ascii
    /// </summary>
    /// <param name="str">string</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <returns>ascii code</returns>
    public static string? ToAscii(this string str)
    {
        var array = Encoding.ASCII.GetBytes(str);
        string? asciiStr2 = null;
        if (asciiStr2 == null) throw new ArgumentNullException(nameof(asciiStr2));
        foreach (var t in array)
        {
            int asciicode = t;
            asciiStr2 += Convert.ToString(asciicode);
        }

        return asciiStr2;
    }


    /// <summary>
    ///     StringToBase64String
    /// </summary>
    /// <param name="str">string</param>
    /// <returns>Base64String</returns>
    public static string StringToBase64String(this string str)
    {
        var bt = Encoding.UTF8.GetBytes(str);
        return Convert.ToBase64String(bt);
    }

    /// <summary>
    ///     Base64StringToString
    /// </summary>
    /// <param name="str">Base64String</param>
    /// <returns>String</returns>
    public static string Base64StringToString(this string str)
    {
        var bt = Convert.FromBase64String(str);
        return Encoding.UTF8.GetString(bt);
    }

    /// <summary>
    ///     格式化json字符串
    /// </summary>
    /// <param name="str">json string</param>
    /// <returns>json format string</returns>
    public static string ToJsonFormatString(this string str)
    {
        //格式化json字符串
        var serializer = new JsonSerializer();
        TextReader tr = new StringReader(str);
        var jtr = new JsonTextReader(tr);
        var obj = serializer.Deserialize(jtr);
        if (obj != null)
        {
            var textWriter = new StringWriter();
            var jsonWriter = new JsonTextWriter(textWriter)
            {
                Formatting = Formatting.Indented,
                Indentation = 4,
                IndentChar = ' '
            };
            serializer.Serialize(jsonWriter, obj);
            return textWriter.ToString();
        }

        return str;
    }

    private static XmlDocument GetXmlDocument(string xmlString)
    {
        var document = new XmlDocument();
        document.LoadXml(xmlString);
        return document;
    }

    private static string ConvertXmlDocumentTostring(XmlDocument xmlDocument)
    {
        var memoryStream = new MemoryStream();
        var writer = new XmlTextWriter(memoryStream, null)
        {
            Formatting = System.Xml.Formatting.Indented //缩进
        };
        xmlDocument.Save(writer);
        var streamReader = new StreamReader(memoryStream);
        memoryStream.Position = 0;
        var xmlString = streamReader.ReadToEnd();
        streamReader.Close();
        memoryStream.Close();
        return xmlString;
    }

    /// <summary>
    ///     to xml string
    /// </summary>
    /// <param name="xmLString">xml string</param>
    /// <returns>xml format string</returns>
    public static string ToXmlFormatString(this string xmLString)
    {
        var xmlDocument = GetXmlDocument(xmLString);
        return ConvertXmlDocumentTostring(xmlDocument);
    }

    /// <summary>
    ///     string转16进制
    /// </summary>
    /// <param name="strAscii">string to ascii</param>
    /// <param name="separator">分隔符</param>
    /// <returns>16进制的string</returns>
    public static string ConvertStringToHex(this string strAscii, string? separator = null)
    {
        var sbHex = new StringBuilder();
        foreach (var chr in strAscii)
        {
            sbHex.Append(string.Format("{0:X2}", Convert.ToInt32(chr)));
            sbHex.Append(separator ?? string.Empty);
        }

        return sbHex.ToString();
    }

    /// <summary>
    ///     16进制转string
    /// </summary>
    /// <param name="hexValue">16进制string</param>
    /// <param name="separator">分隔符</param>
    /// <returns>string说</returns>
    public static string ConvertHexToString(this string hexValue, string? separator = null)
    {
        hexValue = string.IsNullOrEmpty(separator) ? hexValue : hexValue.Replace(string.Empty, separator);
        var sbStrValue = new StringBuilder();
        while (hexValue.Length > 0)
        {
            sbStrValue.Append(Convert.ToChar(Convert.ToUInt32(hexValue.Substring(0, 2), 16)).ToString());
            hexValue = hexValue.Substring(2);
        }

        return sbStrValue.ToString();
    }

    /// <summary>
    ///     将指定的16进制字符串转换为byte数组
    /// </summary>
    /// <param name="s">16进制字符串(如：“7F 2C 4A”或“7F2C4A”都可以)</param>
    /// <returns>16进制字符串对应的byte数组</returns>
    public static byte[] ConvertHexStringToByteArray(this string s)
    {
        s = s.Replace(" ", "");
        var buffer = new byte[s.Length / 2];
        for (var i = 0; i < s.Length; i += 2)
            buffer[i / 2] = Convert.ToByte(s.Substring(i, 2), 16);
        return buffer;
    }

    /// <summary>
    ///     string to bytes
    /// </summary>
    /// <param name="str">string</param>
    /// <returns>byte array</returns>
    public static byte[] ConvertStringToBytes(this string str)
    {
        return Encoding.Default.GetBytes(str);
    }

    /// <summary>
    ///     Utf8ToGb2312
    /// </summary>
    /// <param name="text">string</param>
    /// <returns>Gb2312 string</returns>
    public static string Utf8ToGb2312(this string text)
    {
        //声明字符集   
        //utf8   
        var utf8 = Encoding.GetEncoding("utf-8");
        //gb2312   
        var gb2312 = Encoding.GetEncoding("gb2312");
        byte[] utf;
        utf = utf8.GetBytes(text);
        utf = Encoding.Convert(utf8, gb2312, utf);
        //返回转换后的字符   
        return gb2312.GetString(utf);
    }

    /// <summary>
    ///     StringEncode
    /// </summary>
    /// <param name="text">string</param>
    /// <param name="sourceEncode">source encode</param>
    /// <param name="transEncode">trans Encode</param>
    /// <returns>string</returns>
    public static string? ConvertStringEncode(this string? text, Encoding sourceEncode, Encoding transEncode)
    {
        var utf = sourceEncode.GetBytes(text ?? throw new ArgumentNullException(nameof(text)));
        utf = Encoding.Convert(sourceEncode, transEncode, utf);
        //返回转换后的字符   
        return transEncode.GetString(utf);
    }

    /// <summary>
    ///     string to Enum
    /// </summary>
    /// <param name="text"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ConvertStringToEnum<T>(this string text)
    {
        return (T)Enum.Parse(typeof(T), text);
    }

    /// <summary>
    ///     QueryStringToJson
    /// </summary>
    /// <param name="queryString">http request queryString</param>
    /// <returns>json string</returns>
    public static string QueryStringToJson(this string queryString)
    {
        var strlst = queryString.Split('&').ToList();
        var jsonObject = new JObject();
        foreach (var item in strlst)
        {
            var key = item.Split('=')[0];
            var value = item.Split('=')[1];
            jsonObject.Add(key, value);
        }

        return jsonObject.ToString();
    }

    /// <summary>
    ///     ConvertStringToDateTime
    /// </summary>
    /// <param name="dateTimeStr">dateTimeStr</param>
    /// <param name="dateTimeFormat">dateTimeFormat</param>
    /// <returns>DateTime</returns>
    public static DateTime ConvertStringToDateTime(this string dateTimeStr, string dateTimeFormat)
    {
        var dt = DateTime.ParseExact(dateTimeStr, dateTimeFormat, CultureInfo.CurrentCulture);
        return dt;
    }

    /// <summary>
    ///     ConvertStringEnumerableToEnums
    /// </summary>
    /// <param name="strs">dateTimeStr</param>
    /// <returns>DateTime</returns>
    public static IEnumerable<T> ConvertStringEnumerableToEnums<T>(this IEnumerable<string> strs)
    {
        var lst = new List<T>();
        foreach (var str in strs) lst.Add((T)Enum.Parse(typeof(T), str));

        return lst;
    }

    /// <summary>
    ///     string sha256 加密
    /// </summary>
    /// <param name="str">需要加密的字符串</param>
    /// <returns>sha256加密后的值</returns>
    [Obsolete("Obsolete")]
    public static string Sha256(this string str)
    {
        Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(str));

        var sha256Data = Encoding.UTF8.GetBytes(str);
        var sha256 = new SHA256Managed();
        var by = sha256.ComputeHash(sha256Data);
        var result = BitConverter.ToString(by).Replace("-", "").ToLower(); //64
        return result;
    }

    /// <summary>
    ///     微信掩码
    ///     <code>
    /// 掩码规则： AA******  保留前两位，后续全部使用 * 掩码
    /// </code>
    /// </summary>
    /// <param name="entity">用户对象</param>
    /// <param name="fieldName">需要掩码的字段</param>
    /// <param name="newFieldName">掩码后的新字段</param>
    /// <returns>具有掩码字段的对象</returns>
    public static JObject AddWxNameMark(this object entity, string fieldName, string newFieldName)
    {
        var wxName = entity.GetType().GetProperty(fieldName)?.GetValue(entity)?.ToString();
        string? wxNameMark = null;
        var star = "";
        if (wxName != null)
            for (var i = 0; i < wxName.Length - 2; i++)
                star += "*";

        wxNameMark = wxName?.Substring(0, 2) + star;
        var jobj = JObject.Parse(JsonConvert.SerializeObject(entity));
        jobj.Add(newFieldName ?? $"Mark{fieldName}", wxNameMark);
        return jobj;
    }

    /// <summary>
    ///     邮箱掩码
    ///     <code>
    /// 掩码规则：如果 @前的邮箱名 length>3 那么 A***B 收尾保留其余用 * 代替， 否则 不做掩码处理
    /// </code>
    /// </summary>
    /// <param name="entity">用户对象</param>
    /// <param name="fieldName">需要掩码的字段</param>
    /// <param name="newfieldName">掩码后的新字段</param>
    /// <returns>具有掩码字段的对象</returns>
    public static JObject AddMailMark(this object entity, string fieldName, string newfieldName)
    {
        var mail = entity.GetType().GetProperty(fieldName)?.GetValue(entity)?.ToString()?.Split('@')[0];
        if (mail != null && mail.IsEmail())
        {
            string? mailMark = null;
            if (mail.Length > 3)
            {
                var star = "";
                for (var i = 0; i < mail.Length - 2; i++) star += "*";

                mailMark = mail.Substring(0, 1) + star + mail.Substring(mail.Length - 1, 1);
            }
            else
            {
                mailMark = mail;
            }

            var jobj = JObject.Parse(JsonConvert.SerializeObject(entity));
            jobj.Add(newfieldName ?? $"Mark{fieldName}", mailMark);
            return jobj;
        }

        throw new ColaException(EnumException.RegexEmail);
    }

    /// <summary>
    ///     姓名掩码
    ///     <code>
    /// 掩码规则：1. length=2 A*  2. length=3  A*b   3. 其余 A**B，收尾保留其余用 * 代替
    /// </code>
    /// </summary>
    /// <param name="entity">用户对象</param>
    /// <param name="fieldName">需要掩码的字段</param>
    /// <param name="newfieldName">掩码后的新字段</param>
    /// <returns>具有掩码字段的对象</returns>
    public static JObject AddNameMark(this object entity, string fieldName, string newfieldName)
    {
        var userName = entity.GetType().GetProperty(fieldName)?.GetValue(entity)?.ToString();
        string? userNameMark = null;
        if (userName != null && userName.Length == 2)
        {
            userNameMark = userName.Substring(0, 1) + "*";
        }
        else if (userName != null && userName.Length == 3)
        {
            userNameMark = userName.Substring(0, 1) + "*" + userName.Substring(2, 1);
        }
        else
        {
            var star = "";
            if (userName != null)
                for (var i = 0; i < userName.Length - 2; i++)
                    star += "*";

            userNameMark = userName?.Substring(0, 1) + star + userName?.Substring(userName.Length - 1, 1);
        }

        var jobj = JObject.Parse(JsonConvert.SerializeObject(entity));
        jobj.Add(newfieldName ?? $"Mark{fieldName}", userNameMark);
        return jobj;
    }

    /// <summary>
    ///     身份证掩码
    ///     <code>
    /// 掩码规则：  ABCDEFGHIJ*******K, 获取前10位和最后一位，奇遇用 掩码 代码
    /// </code>
    /// </summary>
    /// <param name="entity">用户对象</param>
    /// <param name="fieldName">需要掩码的字段</param>
    /// <param name="newfieldName">掩码后的新字段</param>
    /// <param name="strMark"></param>
    /// <returns>具有掩码字段的对象</returns>
    public static JObject AddCardIdMark(this object entity, string fieldName, string? newfieldName = null,
        string? strMark = null)
    {
        var cardId = entity.GetType().GetProperty(fieldName)?.GetValue(entity)?.ToString();
        if (cardId != null && cardId.IsChinaCardId())
        {
            var cardIdMark = Regex.Replace(cardId, StringRegexExtensions.RegexChinaCardId,
                strMark ?? StringRegexExtensions.StringMarkChinaCardId);
            var jobj = JObject.Parse(JsonConvert.SerializeObject(entity));
            jobj.Add(newfieldName ?? $"Mark{fieldName}", cardIdMark);
            return jobj;
        }

        throw new ColaException(EnumException.RegexIdCardNumber);
    }

    /// <summary>
    ///     电话号码掩码
    ///     <code>
    /// 掩码规则：  ABC****DEFGHIJ,  前3位 和 后7位，其余用 掩码 代替
    /// </code>
    /// </summary>
    /// <param name="entity">用户对象</param>
    /// <param name="fieldName">需要掩码的字段</param>
    /// <param name="newfieldName">掩码后的新字段</param>
    /// <param name="strMark"></param>
    /// <returns>具有掩码字段的对象</returns>
    public static JObject AddPhoneMark(this object entity, string fieldName, string? newfieldName = null,
        string? strMark = null)
    {
        var phone = entity.GetType().GetProperty(fieldName)?.GetValue(entity)?.ToString();
        if (phone != null && phone.IsChinaMobile())
        {
            var phoneMark = Regex.Replace(phone, StringRegexExtensions.RegexChinaMobile,
                strMark ?? StringRegexExtensions.StringMarkChinaMobile);
            var jobj = JObject.Parse(JsonConvert.SerializeObject(entity));
            jobj.Add(newfieldName ?? $"Mark{fieldName}", phoneMark);
            return jobj;
        }

        throw new ColaException(EnumException.RegexPhoneNumber);
    }

    /// <summary>
    ///     md5 加密 转小写
    /// </summary>
    /// <param name="str">需要加密的字符串</param>
    /// <param name="salt"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static string ToMd5Lower(this string str, string? salt = null, int length = 32)
    {
        var md5 = MD5.Create();
        var bt = md5.ComputeHash(Encoding.UTF8.GetBytes(str + (salt.IsNullOrEmpty() ? "" : salt)));
        var sb = new StringBuilder();
        for (var i = 0; i < bt.Length; i++) sb.AppendFormat("{0:x2}", bt[i]);

        return sb.ToString().ToLower().Substring(0, length);
    }

    /// <summary>
    ///     两次 md5 加密 转小写
    ///     <code>
    /// e.g (str+salt).md5().md5()
    /// </code>
    /// </summary>
    /// <param name="str">需要加密的字符串</param>
    /// <param name="salt"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static string ToMd5Lower2(this string str, string? salt = null, int length = 32)
    {
        return str.ToMd5Lower(salt).ToMd5Lower();
    }

    /// <summary>
    ///     md5 加密 转大写
    /// </summary>
    /// <param name="str">需要加密的字符串</param>
    /// <param name="salt"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static string ToMd5Upper(this string str, string? salt = null, int length = 32)
    {
        var md5 = MD5.Create();
        var bt = md5.ComputeHash(Encoding.UTF8.GetBytes(str + (salt.IsNullOrEmpty() ? "" : salt)));
        var sb = new StringBuilder();
        for (var i = 0; i < bt.Length; i++) sb.AppendFormat("{0:x2}", bt[i]);

        return sb.ToString().ToUpper().Substring(0, length);
    }

    /// <summary>
    ///     两次 md5 加密 转大写
    ///     <code>
    /// e.g (str+salt).md5().md5()
    /// </code>
    /// </summary>
    /// <param name="str">需要加密的字符串</param>
    /// <param name="salt"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static string ToMd5Upper2(this string str, string? salt = null, int length = 32)
    {
        return str.ToMd5Upper(salt).ToMd5Upper();
    }
}