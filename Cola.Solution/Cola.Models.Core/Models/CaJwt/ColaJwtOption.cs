namespace Cola.Models.Core.Models.CaJwt;
public class ColaJwtOption
{
    public string Secret { get; set; } = "Cola.Jwt.Secret";
    public string IsUser { get; set; } = "www.odinsam.com";
    public string Audience { get; set; } = "odinsam";
    public int AccessExpiration { get; set; } = 30;
    public int RefreshExpiration { get; set; } = 1440;
    public int RenewalExpir { get; set; } = 30;
}
