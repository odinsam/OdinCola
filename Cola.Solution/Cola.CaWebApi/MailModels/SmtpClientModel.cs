namespace Cola.CaWebApi.MailModels;

public class SmtpClientModel
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string CredentialsUserName { get; set; }
    public string CredentialsUserPwd { get; set; }
    public bool EnableSsl { get; set; } = true;
}