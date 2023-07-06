namespace Cola.Models.Core.Models.CaJwt;

public class TokenResult
{
    public AccessToken Token { get; set; }
    
    public RefreshToken RefreshToken { get; set; }
}