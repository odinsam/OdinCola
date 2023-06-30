namespace Cola.CaUtils.Models.MailSendModels;

/// <summary>
///     MailSendModel
/// </summary>
public class MailSendModel
{
    public MailSendModel(bool enableSsl = false)
    {
        SendConfig = new SendMailServerConfig();
        SendConfig.EnableSsl = enableSsl;
    }

    public SendMailServerConfig SendConfig { get; set; }
    public string? Subject { get; set; }
    public string MailDateTime { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    public int Templateid { get; set; }
    public MailFromUserModel? FromUser { get; set; }

    /// <summary>
    ///     ToUsers
    /// </summary>
    public List<MailToUserModel>? ToUsers { get; set; }

    public List<MailCcUserModel>? CcUsers { get; set; }
    public List<string>? Files { get; set; }
    public string? Content { get; set; }
}