using System.Security.Cryptography;

namespace Cola.CaUtils.Extensions;

public static class SecurityExtensions
{
    /// <summary>
    ///     stream sha256 加密
    /// </summary>
    /// <param name="stream">需要加密的流</param>
    /// <returns>sha256加密后的string信息</returns>
    public static string Sha256(this Stream stream)
    {
        using var sha256 = SHA256.Create();
        var by = sha256.ComputeHash(stream);
        var result = BitConverter.ToString(by).Replace("-", "").ToLower(); //64
        return result;
    }
}