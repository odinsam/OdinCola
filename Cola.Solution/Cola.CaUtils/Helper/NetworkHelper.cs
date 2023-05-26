using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Cola.CaUtils.Helper;

/// <summary>
///     NetworkHelper
/// </summary>
public class NetworkHelper
{
    /// <summary>
    ///     GetHostIpForFas
    /// </summary>
    /// <returns></returns>
    public static IList<string>? GetHostIpForFas()
    {
        try
        {
            IList<string>? strIp = new List<string>();
            //NetworkInterface：提供网络接口的配置和统计信息。
            var adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var adapter in adapters)
            {
                var adapterProperties = adapter.GetIPProperties();
                var allAddress = adapterProperties.UnicastAddresses;
                //这里是根据网络适配器名称找到对应的网络，adapter.Name即网络适配器的名称
                if (allAddress.Count > 0 && adapter.Name == "WLAN 2")
                    foreach (var addr in allAddress)
                        if (addr.Address.AddressFamily == AddressFamily.InterNetwork)
                            strIp.Add(addr.Address.ToString());
            }

            return strIp;
        }
        catch (Exception)
        {
            return null;
        }
    }
}