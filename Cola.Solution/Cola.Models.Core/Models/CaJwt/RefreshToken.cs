namespace Cola.Models.Core.Models.CaJwt;
public class RefreshToken
{
    public string TokenId { get; set; }

    public string JwtId { get; set; }

    public string Token { get; set; }

    /// <summary>
    /// 是否使用，一个RefreshToken只能使用一次
    /// </summary>
    public bool Used { get; set; } = false;

    /// <summary>
    /// 是否失效。修改用户重要信息时可将此字段更新为true，使用户重新登录
    /// </summary>
    public bool Invalidated { get; set; } = false;

    public DateTime CreationTime { get; set; }

    /// <summary>
    /// 过期时间
    /// </summary>
    public DateTime ExpiryTime { get; set; }

    /// <summary>
    /// 是否续约 达到续约时限则为true，否则为false
    /// </summary>
    /// <param name="renewalExpir">续约时限</param>
    /// <returns></returns>
    public bool IsRenewal(int renewalExpir) => (ExpiryTime - DateTime.Now).TotalMinutes < renewalExpir;
}
