namespace Lithium.Website;

public class SmtpConfiguration
{
    public string SmtpServer { get; set; }
    public int Port { get; set; }
    public bool SslEnabled { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string From { get; set; }
}