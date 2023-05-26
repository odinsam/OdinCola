using System.Collections;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;

namespace Cola.CaUtils.Helper;

public class RegexHelper
{
    /// <summary>
    ///     可以进行判断的类型
    /// </summary>
    public enum Operation
    {
        Byte,
        SByte,
        Int16,
        Int32,
        Int64,
        Single,
        Double,
        Boolean,
        Char,
        Decimal,
        DateTime,
        Date,
        Time,
        EMail,
        Url,
        ChinaPhone,
        ChineseWord,
        ChinesePostalCode,
        Number,
        StringModel01,
        StringModel02,
        WideWord,
        NarrowWord,
        IpAddress,
        ChineseMobile,
        ChineseId
    }

    private bool _isEntirety;
    private string? _string;


    #region 用于判断字符串是否是对应类型（默认为包含匹配）

    /// <summary>
    ///     用于判断字符串是否是对应类型
    ///     <code>
    /// MSCL.RegHelper rm = new MSCL.RegHelper();
    /// Response.Write(rm.IsAccordType("123", MSCL.RegHelper.Operation.ChineseWord));
    /// </code>
    /// </summary>
    /// <param name="strVerifyString">String，需要判断的字符串</param>
    /// <param name="op">Operation枚举，用于选择需要进行的操作</param>
    /// <returns></returns>
    public bool IsAccordType(string strVerifyString, Operation op)
    {
        return IsAccordType(strVerifyString, op, false);
    }

    #endregion

    #region 用于判断字符串是否是对应类型（或是否包含对应类型的字符）

    /// <summary>
    ///     用于判断字符串是否是对应类型
    /// </summary>
    /// <param name="strVerifyString">String，需要判断的字符串</param>
    /// <param name="op">Operation枚举，用于选择需要进行的操作</param>
    /// <param name="isEntirety">Boolean，判断是完全匹配还是包含匹配模式（仅适用于非类型判断时）</param>
    /// <returns></returns>
    public bool IsAccordType(string strVerifyString, Operation op, bool isEntirety)
    {
        _string = strVerifyString;
        _isEntirety = isEntirety;

        switch (op)
        {
            case Operation.Byte:
            {
                return IsByte();
            }
            case Operation.SByte:
            {
                return IsSByte();
            }
            case Operation.Int16:
            {
                return IsInt16();
            }
            case Operation.Int32:
            {
                return IsInt32();
            }
            case Operation.Int64:
            {
                return IsInt64();
            }
            case Operation.Single:
            {
                return IsSingle();
            }
            case Operation.Double:
            {
                return IsDouble();
            }
            case Operation.Boolean:
            {
                return IsBoolean();
            }
            case Operation.Char:
            {
                return IsChar();
            }
            case Operation.Decimal:
            {
                return IsDecimal();
            }
            case Operation.DateTime:
            {
                return IsDateTime();
            }
            case Operation.Date:
            {
                return IsDate();
            }
            case Operation.Time:
            {
                return IsTime();
            }
            case Operation.IpAddress:
            {
                return IsIpAddress();
            }
            case Operation.ChinaPhone:
            {
                return IsChinaPhone();
            }
            case Operation.ChinesePostalCode:
            {
                return IsChinesePostalCode();
            }
            case Operation.ChineseMobile:
            {
                return IsChineseMobile();
            }
            case Operation.EMail:
            {
                return IsEmail();
            }
            case Operation.Url:
            {
                return IsURL();
            }
            case Operation.ChineseWord:
            {
                return IsChineseWord();
            }
            case Operation.Number:
            {
                return IsNumber();
            }
            case Operation.StringModel01:
            {
                return IsStringModel_01();
            }
            case Operation.StringModel02:
            {
                return IsStringModel_02();
            }
            case Operation.WideWord:
            {
                return IsWideWord();
            }
            case Operation.NarrowWord:
            {
                return IsNarrowWord();
            }
            case Operation.ChineseId:
            {
                return IsChineseId();
            }
            default:
            {
                return false;
            }
        }
    }

    #endregion

    #region 通用正则表达式判断函数

    /// <summary>
    ///     通用正则表达式判断函数
    /// </summary>
    /// <param name="strVerifyString">String，用于匹配的字符串</param>
    /// <param name="strRegular">String，正则表达式</param>
    /// <param name="regOption">RegexOptions，配置正则表达式的选项</param>
    /// <param name="aryResult">ArrayList，分解的字符串内容</param>
    /// <param name="isEntirety">Boolean，是否需要完全匹配</param>
    /// <returns></returns>
    public bool CommRegularMatch(string strVerifyString, string strRegular,
        RegexOptions regOption,
        ref ArrayList aryResult, bool isEntirety)
    {
        Regex r;
        Match m;

        #region 如果需要完全匹配的处理

        if (isEntirety)
        {
            strRegular = strRegular.Insert(0, @"\A");
            strRegular = strRegular.Insert(strRegular.Length, @"\z");
        }

        #endregion

        r = new Regex(strRegular, regOption);

        for (m = r.Match(strVerifyString); m.Success; m = m.NextMatch()) aryResult.Add(m);

        if (aryResult.Count == 0)
            return false;
        return true;
    }

    #endregion

    #region 中国身份证号码验证

    private string CheckCidInfo(string cid)
    {
        string?[] aCity =
        {
            null, null, null, null, null, null, null, null, null, null, null, "北京", "天津", "河北", "山西", "内蒙古", null, null,
            null, null, null, "辽宁", "吉林", "黑龙江", null, null, null, null, null, null, null, "上海", "江苏", "浙江", "安微", "福建",
            "江西", "山东", null, null, null, "河南", "湖北", "湖南", "广东", "广西", "海南", null, null, null, "重庆", "四川", "贵州", "云南",
            "西藏", null, null, null, null, null, null, "陕西", "甘肃", "青海", "宁夏", "新疆", null, null, null, null, null, "台湾",
            null, null, null, null, null, null, null, null, null, "香港", "澳门", null, null, null, null, null, null, null,
            null, "国外"
        };
        double iSum = 0;
        var info = string.Empty;
        var rg = new Regex(@"^\d{17}(\d|x)$");
        var mc = rg.Match(cid);
        if (!mc.Success) return string.Empty;

        cid = cid.ToLower();
        cid = cid.Replace("x", "a");
        if (aCity[int.Parse(cid.Substring(0, 2))] == null) return "非法地区";

        try
        {
            var dateTime =
                DateTime.Parse(cid.Substring(6, 4) + " - " + cid.Substring(10, 2) + " - " + cid.Substring(12, 2));
        }
        catch
        {
            return "非法生日";
        }

        for (var i = 17; i >= 0; i--)
            iSum += Math.Pow(2, i) % 11 *
                    int.Parse(cid[17 - i].ToString(), NumberStyles.HexNumber);

        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (iSum % 11 != 1)
            return "非法证号";
        return aCity[int.Parse(cid.Substring(0, 2))] + "," + cid.Substring(6, 4) + "-" + cid.Substring(10, 2) +
               "-" + cid.Substring(12, 2) + "," + (int.Parse(cid.Substring(16, 1)) % 2 == 1 ? "男" : "女");
    }

    #endregion

    #region 身份证号码15升级为18位

    private string CidUpdate(string shortCid)
    {
        char[] strJiaoYan = { '1', '0', 'X', '9', '8', '7', '6', '5', '4', '3', '2' };
        int[] intQuan = { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2, 1 };
        string strTemp;
        var intTemp = 0;

        strTemp = shortCid.Substring(0, 6) + "19" + shortCid.Substring(6);
        for (var i = 0; i <= strTemp.Length - 1; i++) intTemp += int.Parse(strTemp.Substring(i, 1)) * intQuan[i];

        intTemp = intTemp % 11;
        return strTemp + strJiaoYan[intTemp];
    }

    #endregion

    #region 具体验证方法

    #region 是否Byte类型（8 位的无符号整数）： 0 和 255 之间的无符号整数

    /**/
    /// <summary>
    ///     是否Byte类型（8 位的无符号整数）： 0 和 255 之间的无符号整数
    /// </summary>
    /// <returns>Boolean</returns>
    protected bool IsByte()
    {
        try
        {
            if (_string != null)
            {
                var b = byte.Parse(_string);
            }
        }
        catch
        {
            return false;
        }

        return true;
    }

    #endregion

    #region 是否SByte类型（8 位的有符号整数）： -128 到 +127 之间的整数

    /**/
    /// <summary>
    ///     是否SByte类型（8 位的有符号整数）： -128 到 +127 之间的整数
    /// </summary>
    /// <returns>Boolean</returns>
    protected bool IsSByte()
    {
        try
        {
            if (_string != null)
            {
                var @sbyte = sbyte.Parse(_string);
            }
        }
        catch
        {
            return false;
        }

        return true;
    }

    #endregion

    #region 是否Int16类型（16 位的有符号整数）： -32768 到 +32767 之间的有符号整数

    /**/
    /// <summary>
    ///     是否Int16类型（16 位的有符号整数）： -32768 到 +32767 之间的有符号整数
    /// </summary>
    /// <returns>Boolean</returns>
    protected bool IsInt16()
    {
        try
        {
            if (_string != null)
            {
                var s = short.Parse(_string);
            }
        }
        catch
        {
            return false;
        }

        return true;
    }

    #endregion

    #region 是否Int32类型（32 位的有符号整数）：-2,147,483,648 到 +2,147,483,647 之间的有符号整数

    /**/
    /// <summary>
    ///     是否Int32类型（32 位的有符号整数）：-2,147,483,648 到 +2,147,483,647 之间的有符号整数
    /// </summary>
    /// <returns>Boolean</returns>
    protected bool IsInt32()
    {
        try
        {
            if (_string != null)
            {
                var i = int.Parse(_string);
            }
        }
        catch
        {
            return false;
        }

        return true;
    }

    #endregion

    #region 是否Int64类型（64 位的有符号整数）： -9,223,372,036,854,775,808 到 +9,223,372,036,854,775,807 之间的整数

    /**/
    /// <summary>
    ///     是否Int64类型（64 位的有符号整数）： -9,223,372,036,854,775,808 到 +9,223,372,036,854,775,807 之间的整数
    /// </summary>
    /// <returns>Boolean</returns>
    protected bool IsInt64()
    {
        try
        {
            if (_string != null)
            {
                var l = long.Parse(_string);
            }
        }
        catch
        {
            return false;
        }

        return true;
    }

    #endregion

    #region 是否Single类型（单精度（32 位）浮点数字）： -3.402823e38 和 +3.402823e38 之间的单精度 32 位数字

    /**/
    /// <summary>
    ///     是否Single类型（单精度（32 位）浮点数字）： -3.402823e38 和 +3.402823e38 之间的单精度 32 位数字
    /// </summary>
    /// <returns>Boolean</returns>
    protected bool IsSingle()
    {
        try
        {
            if (_string != null)
            {
                var f = float.Parse(_string);
            }
        }
        catch
        {
            return false;
        }

        return true;
    }

    #endregion

    #region 是否Double类型（单精度（64 位）浮点数字）： -1.79769313486232e308 和 +1.79769313486232e308 之间的双精度 64 位数字

    /**/
    /// <summary>
    ///     是否Double类型（单精度（64 位）浮点数字）： -1.79769313486232e308 和 +1.79769313486232e308 之间的双精度 64 位数字
    /// </summary>
    /// <returns>Boolean</returns>
    protected bool IsDouble()
    {
        try
        {
            if (_string != null)
            {
                var d = double.Parse(_string);
            }
        }
        catch
        {
            return false;
        }

        return true;
    }

    #endregion

    #region 是否Boolean类型（布尔值）：true 或 false

    /**/
    /// <summary>
    ///     是否Double类型（单精度（64 位）浮点数字）： -1.79769313486232e308 和 +1.79769313486232e308 之间的双精度 64 位数字
    /// </summary>
    /// <returns>Boolean</returns>
    protected bool IsBoolean()
    {
        try
        {
            if (_string != null)
            {
                var b = bool.Parse(_string);
            }
        }
        catch
        {
            return false;
        }

        return true;
    }

    #endregion

    #region 是否Char类型（Unicode（16 位）字符）：该 16 位数字的值范围为从十六进制值 0x0000 到 0xFFFF

    /**/
    /// <summary>
    ///     是否Char类型（Unicode（16 位）字符）：该 16 位数字的值范围为从十六进制值 0x0000 到 0xFFFF
    /// </summary>
    /// <returns>Boolean</returns>
    protected bool IsChar()
    {
        try
        {
            if (_string != null)
            {
                var c = char.Parse(_string);
            }
        }
        catch
        {
            return false;
        }

        return true;
    }

    #endregion

    #region 是否Char类型（96 位十进制值）：从正 79,228,162,514,264,337,593,543,950,335 到负 79,228,162,514,264,337,593,543,950,335 之间的十进制数

    /**/
    /// <summary>
    ///     是否Char类型（96 位十进制值）：从正 79,228,162,514,264,337,593,543,950,335 到负 79,228,162,514,264,337,593,543,950,335 之间的十进制数
    /// </summary>
    /// <returns>Boolean</returns>
    protected bool IsDecimal()
    {
        try
        {
            if (_string != null)
            {
                var @decimal = decimal.Parse(_string);
            }
        }
        catch
        {
            return false;
        }

        return true;
    }

    #endregion

    #region 是否DateTime类型（表示时间上的一刻）： 范围在公元（基督纪元）0001 年 1 月 1 日午夜 12:00:00 到公元 (C.E.) 9999 年 12 月 31 日晚上 11:59:59 之间的日期和时间

    /**/
    /// <summary>
    ///     是否DateTime类型（表示时间上的一刻）： 范围在公元（基督纪元）0001 年 1 月 1 日午夜 12:00:00 到公元 (C.E.) 9999 年 12 月 31 日晚上 11:59:59 之间的日期和时间
    /// </summary>
    /// <returns>Boolean</returns>
    protected bool IsDateTime()
    {
        try
        {
            if (_string != null)
            {
                var dateTime = DateTime.Parse(_string);
            }
        }
        catch
        {
            return false;
        }

        return true;
    }

    #endregion

    #region 是否Date类型（表示时间的日期部分）： 范围在公元（基督纪元）0001 年 1 月 1 日 到公元 (C.E.) 9999 年 12 月 31 日之间的日期

    /**/
    /// <summary>
    ///     是否Date类型（表示时间的日期部分）： 范围在公元（基督纪元）0001 年 1 月 1 日 到公元 (C.E.) 9999 年 12 月 31 日之间的日期
    /// </summary>
    /// <returns>Boolean</returns>
    protected bool IsDate()
    {
        DateTime value;
        try
        {
            if (_string == null)
                throw new Exception("_string 不存在");
            value = DateTime.Parse(_string);
        }
        catch
        {
            return false;
        }

        if (value.Date.ToString(CultureInfo.InvariantCulture) == _string)
            return true;
        return false;
    }

    #endregion

    #region 是否Time类型（表示时间部分HHMMSS）： 范围在夜 12:00:00 到晚上 11:59:59 之间的时间

    /**/
    /// <summary>
    ///     是否Time类型（表示时间部分HHMMSS）： 范围在夜 12:00:00 到晚上 11:59:59 之间的时间
    /// </summary>
    /// <returns>Boolean</returns>
    protected bool IsTime()
    {
        DateTime Value;
        try
        {
            if (_string == null)
                throw new Exception("_string 不存在");
            Value = DateTime.Parse(_string);
        }
        catch
        {
            return false;
        }

        if (Value.Year == 1 && Value.Month == 1 && Value.Day == 1)
            return true;
        return false;
    }

    #endregion

    #region 是否IPAddress类型（IPv4 的情况下使用以点分隔的四部分表示法格式表示，IPv6 的情况下使用冒号与十六进制格式表示）

    /**/
    /// <summary>
    ///     是否IPAddress类型（IPv4 的情况下使用以点分隔的四部分表示法格式表示，IPv6 的情况下使用冒号与十六进制格式表示）
    /// </summary>
    /// <returns>Boolean</returns>
    protected bool IsIpAddress()
    {
        try
        {
            if (_string != null) IPAddress.Parse(_string);
        }
        catch
        {
            return false;
        }

        return true;
    }

    #endregion

    #region 是否中国电话号码类型（XXX/XXXX-XXXXXXX/XXXXXXXX (\d{3,4})-?\d{7,8}）：判断是否是（区号：3或4位）-（电话号码：7或8位）

    /**/
    /// <summary>
    ///     是否中国电话号码类型（XXX/XXXX-XXXXXXX/XXXXXXXX (\d{3,4})-?\d{7,8}）：判断是否是（区号：3或4位）-（电话号码：7或8位）
    /// </summary>
    /// <returns>Boolean</returns>
    protected bool IsChinaPhone()
    {
        var aryResult = new ArrayList();
        return _string != null &&
               CommRegularMatch(_string, @"(\d{3,4})-?\d{7,8}", RegexOptions.None, ref aryResult, _isEntirety);
    }

    #endregion

    #region 是否中国邮政编码（6位数字 \d{6}）

    /**/
    /// <summary>
    ///     是否中国邮政编码（6位数字 \d{6}）
    /// </summary>
    /// <returns>Boolean</returns>
    protected bool IsChinesePostalCode()
    {
        var aryResult = new ArrayList();
        return _string != null && CommRegularMatch(_string, @"\d{6}", RegexOptions.None, ref aryResult, _isEntirety);
    }

    #endregion

    #region 是否中国移动/联通/电信的电话号码（13开头的总11位数字 ^1[3456789]\d{9}$）

    /**/
    /// <summary>
    ///     是否中国移动电话号码（13开头的总11位数字 13\d{9}）
    /// </summary>
    /// <returns>Boolean</returns>
    protected bool IsChineseMobile()
    {
        var aryResult = new ArrayList();
        return _string != null &&
               CommRegularMatch(_string, @"^1[3456789]\d{9}$", RegexOptions.None, ref aryResult, _isEntirety);
    }

    #endregion

    #region 是否EMail类型（XXX@XXX.XXX \w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*）

    /**/
    /// <summary>
    ///     是否EMail类型（XXX@XXX.XXX \w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*）
    /// </summary>
    /// <returns>Boolean</returns>
    protected bool IsEmail()
    {
        var aryResult = new ArrayList();
        return _string != null && CommRegularMatch(_string, @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
            RegexOptions.None,
            ref aryResult, _isEntirety);
    }

    #endregion

    #region 是否Internet URL地址类型（http: //）  

    /**/
    /// <summary>
    ///     是否Internet URL地址类型（http://）
    /// </summary>
    /// <returns>Boolean</returns>
    protected bool IsURL()
    {
        var aryResult = new ArrayList();
        return _string != null && CommRegularMatch(_string, @"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?",
            RegexOptions.None, ref aryResult,
            _isEntirety);
    }

    #endregion

    #region 是否中文字符（[\u4e00-\u9fa5]）

    /**/
    /// <summary>
    ///     是否中文字符（[\u4e00-\u9fa5]）
    /// </summary>
    /// <returns>Boolean</returns>
    protected bool IsChineseWord()
    {
        var aryResult = new ArrayList();
        return _string != null &&
               CommRegularMatch(_string, @"[\u4e00-\u9fa5]", RegexOptions.None, ref aryResult, _isEntirety);
    }

    #endregion

    #region 是否是数字（0到9的数字[\d]+）：不包括符号"."和"-"

    /**/
    /// <summary>
    ///     是否是数字（0到9的数字[\d]+）：不包括符号"."和"-"
    /// </summary>
    /// <returns>Boolean</returns>
    protected bool IsNumber()
    {
        var aryResult = new ArrayList();
        return _string != null && CommRegularMatch(_string, @"[\d]+", RegexOptions.None, ref aryResult, _isEntirety);
    }

    #endregion

    #region 是否只包含数字，英文和下划线（[\w]+）

    /**/
    /// <summary>
    ///     是否只包含数字，英文和下划线（[\w]+）
    /// </summary>
    /// <returns>Boolean</returns>
    protected bool IsStringModel_01()
    {
        var aryResult = new ArrayList();
        return _string != null && CommRegularMatch(_string, @"[\w]+", RegexOptions.None, ref aryResult, _isEntirety);
    }

    #endregion

    #region 是否大写首字母的英文字母（[A-Z][a-z]+）

    /**/
    /// <summary>
    ///     是否大写首字母的英文字母（[A-Z][a-z]+）
    /// </summary>
    /// <returns>Boolean</returns>
    protected bool IsStringModel_02()
    {
        var aryResult = new ArrayList();
        return _string != null &&
               CommRegularMatch(_string, @"[A-Z][a-z]+", RegexOptions.None, ref aryResult, _isEntirety);
    }

    #endregion

    #region 是否全角字符（[^\x00-\xff]）：包括汉字在内

    /**/
    /// <summary>
    ///     是否全角字符（[^\x00-\xff]）：包括汉字在内
    /// </summary>
    /// <returns>Boolean</returns>
    protected bool IsWideWord()
    {
        var aryResult = new ArrayList();
        return _string != null &&
               CommRegularMatch(_string, @"[^\x00-\xff]", RegexOptions.None, ref aryResult, _isEntirety);
    }

    #endregion

    #region 是否半角字符（[\x00-\xff]）

    /**/
    /// <summary>
    ///     是否半角字符（[^\x00-\xff]）：包括汉字在内
    /// </summary>
    /// <returns>Boolean</returns>
    protected bool IsNarrowWord()
    {
        var aryResult = new ArrayList();
        return _string != null &&
               CommRegularMatch(_string, @"[\x00-\xff]", RegexOptions.None, ref aryResult, _isEntirety);
    }

    #endregion

    #region 是否合法的中国身份证号码

    /// <summary>
    ///     是否 是身份证号码
    /// </summary>
    /// <returns></returns>
    protected bool IsChineseId()
    {
        if (_string != null && _string.Length == 15) _string = CidUpdate(_string);

        if (_string != null && _string.Length == 18)
        {
            var strResult = CheckCidInfo(_string);
            if (strResult == "非法地区" || strResult == "非法生日" || strResult == "非法证号")
                return false;
            return true;
        }

        return false;
    }

    #endregion

    #endregion
}