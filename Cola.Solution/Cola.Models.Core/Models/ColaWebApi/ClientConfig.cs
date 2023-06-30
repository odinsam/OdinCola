namespace Cola.Models.Core.Models.ColaWebApi;

public class ClientConfig
{
    public string ClientName { get; set; } = null!;
    public string? BaseUri { get; set; } = "";
    public CertConfig? Cert { get; set; }
    public int TimeOut { get; set; }

    /// <summary>
    ///     默认压缩方式  默认压缩方式  None,GZip,Deflate,Brotli,All
    /// </summary>
    public string? Decompression { get; set; } = "GZip";
}