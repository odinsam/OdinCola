namespace Cola.CaUtils.Models.MailSendModels;

/// <summary>
///     SendMailServerConfig
/// </summary>
public class SendMailServerConfig
{
    /// <summary>
    ///     SmtpHost
    /// </summary>
    public string? SmtpHost { get; set; }

    /// <summary>
    ///     SmtpPort
    /// </summary>
    public int SmtpPort { get; set; } = 25;

    /// <summary>
    ///     SmtpUserName
    /// </summary>
    public string? SmtpUserName { get; set; }

    /// <summary>
    ///     SmtpPassword
    /// </summary>
    public string? SmtpPassword { get; set; }

    /// <summary>
    ///     EnableSsl
    /// </summary>
    public bool EnableSsl { get; set; }
}