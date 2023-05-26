using Cola.CaUtils.Enums;
using Cola.CaUtils.Extensions;
using Newtonsoft.Json.Linq;

namespace Cola.CaUtils.Helper;

// *       签名算法 （无需参数   此步骤作废）
// *       UrlAddSign 生成签名       ValiedateSign 验证签名
// *       第一步：对参数按照key=value的格式，并按照参数名ASCII字典序排序:    b=yy&a=xx&c=zz  排序后为  a=xx&b=yy&c=zz
// *       第二步：拼接Url密钥：a=xx&b=yy&c=zz&sign=key     （key由开发者提供）
// *       第三步：Md5ToLower(Url密钥,32) 得到签名秘钥signKey
// *       第四步：生产完整请求参数  a=xx&b=yy&c=zz&sign=signKey
// *       {
// *            "money":30,
// *            "openId":"oJF440nFowxxoEEPLil_h2LyE6Eg",
// *            "orderId":"订单流水号"
// *            "sign":"f9f2finfi2****0jg500g3rgeri",
// *       }
/// <summary>
///     ParamsSignHelper
/// </summary>
public class ParamsSignHelper
{
    private readonly SortedDictionary<string, object?> _mValues = new();

    /// <summary>
    ///     设置某个字段的值
    /// </summary>
    /// <param name="key">key 字段名</param>
    /// <param name="value">value 字段值</param>
    public void SetValue(string key, object? value)
    {
        _mValues[key] = value;
    }


    /// <summary>
    ///     根据字段名获取某个字段的值
    /// </summary>
    /// <param name="key">key 字段名</param>
    /// <returns>key对应的字段值</returns>
    public object? GetValue(string key)
    {
        object? o = null;
        _mValues.TryGetValue(key, out o);
        return o;
    }

    /// 判断某个字段是否已设置
    /// @param key 字段名
    /// <summary>
    ///     判断某个字段是否已设置
    /// </summary>
    /// <param name="key">key 字段名</param>
    /// <returns>若字段key已被设置，则返回true，否则返回false</returns>
    public bool IsSet(string key)
    {
        object? o = null;
        _mValues.TryGetValue(key, out o);
        if (null != o)
            return true;
        return false;
    }


    /// <summary>
    ///     Dictionary格式转化成url参数格式
    /// </summary>
    /// <returns>url格式串, 该串不包含sign字段值</returns>
    public string ToUrl()
    {
        var buff = "";
        foreach (var pair in _mValues)
        {
            if (pair.Value == null)
                throw new ColaException(EnumException.ParamNotNull);
            if (pair.Key != "sign" && pair.Value.ToString() != "") buff += pair.Key + "=" + pair.Value + "&";
        }

        buff = buff.Trim('&');
        return buff;
    }

    /// <summary>
    ///     url添加sign签名
    /// </summary>
    /// <param name="urlParams">url参数字符串</param>
    /// <param name="signKey">签名生成的密钥</param>
    /// <returns>带有sign参数的url</returns>
    public string UrlAddSign(string urlParams, string signKey)
    {
        _mValues.Clear();
        //验证 去sign
        foreach (var urlParam in urlParams.Split('&'))
        {
            if (urlParam.Split('=').Length == 1)
                throw new ColaException(EnumException.ParamNotNull);
            var key = urlParam.Split('=')[0];
            var value = urlParam.Split('=')[1];
            if (key != "sign")
                SetValue(key, value);
        }

        var urlP = ToUrl();
        var urlPsign = $"{urlP}&sign={signKey}";
        var sign = urlPsign.ToMd5Lower();
        urlPsign = urlPsign.Replace(signKey, sign);
        return urlPsign;
    }


    /// <summary>
    ///     验证url sign签名 是否正确
    /// </summary>
    /// <param name="urlParams">url参数字符串</param>
    /// <param name="signKey">签名生成的密钥</param>
    /// <returns>判断url sign参数是否正确</returns>
    public bool ValiedateSign(string urlParams, string signKey)
    {
        _mValues.Clear();
        string? oldSign = null;
        //验证 去sign
        foreach (var urlParam in urlParams.Split('&'))
        {
            if (urlParam.Split('=').Length == 1)
                throw new ColaException(EnumException.ParamNotNull);
            var key = urlParam.Split('=')[0];
            var value = urlParam.Split('=')[1];
            if (key != "sign")
                SetValue(key, value);
            else
                oldSign = value;
        }

        var urlP = ToUrl();
        var urlPsign = $"{urlP}&sign={signKey}";
        var newSign = urlPsign.ToMd5Lower();
        return newSign == oldSign;
    }

    /// <summary>
    ///     验证url sign签名 是否正确
    /// </summary>
    /// <param name="jobj">url参数字jobj对象</param>
    /// <param name="signKey"></param>
    /// <returns>判断url sign参数是否正确</returns>
    public bool ValiedateSign(JObject jobj, string signKey)
    {
        var url = string.Empty;
        if (jobj.Properties().Count() == 1)
            throw new ColaException(EnumException.ParamNotNull);
        _mValues.Clear();
        string? oldSign = null;
        foreach (var item in jobj.Properties())
            if (item.Name != "sign")
                _mValues.Add(item.Name, item.Value);
            else
                oldSign = item.Value.ToString();
        var urlP = ToUrl();
        var urlPsign = $"{urlP}&sign={signKey}";
        var newSign = urlPsign.ToMd5Lower();
        return newSign == oldSign;
    }
}