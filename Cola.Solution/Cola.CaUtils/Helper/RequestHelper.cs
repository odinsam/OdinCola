using Newtonsoft.Json.Linq;

namespace Cola.CaUtils.Helper;

/// <summary>
///     web request helper class
/// </summary>
public class RequestHelper
{
    /// <summary>
    ///     GetRequestParams
    /// </summary>
    /// <param name="strParams">strParams</param>
    /// <param name="method">method</param>
    /// <returns></returns>
    public static JObject? GetRequestParams(string strParams, string method)
    {
        var jobj = new JObject();
        if (method == "GET")
        {
            if (strParams.Length > 0)
            {
                strParams = strParams.Substring(1);
                var aryParams = strParams.Split('&');
                if (aryParams.Length > 0)
                {
                    foreach (var item in aryParams)
                    {
                        var key = item.Split('=')[0];
                        var val = item.Split('=')[1];
                        jobj.Add(key, val);
                    }

                    return jobj;
                }

                return null;
            }

            return null;
        }

        Console.WriteLine(strParams);
        return JObject.Parse(strParams);
    }
}