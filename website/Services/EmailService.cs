using System.Text;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace website.Services;

class EmailService : IEmailService
{
    private readonly ILogger<EmailService> logger;
    private readonly string administratorEmail;
    private readonly string websiteName;
    private readonly string smtpServer;
    private readonly int smtpPort;
    private readonly bool sslEnabled;
    private readonly string smtpUsername;
    private readonly string smtpPassword;

    public EmailService(
        string administratorEmail,
        string websiteName,
        string smtpServer,
        int smtpPort,
        bool sslEnabled,
        string smtpUsername,
        string smtpPassword,
        ILogger<EmailService> logger)
    {
        this.administratorEmail = administratorEmail;
        this.websiteName = websiteName;
        this.smtpServer = smtpServer;
        this.smtpPort = smtpPort;
        this.sslEnabled = sslEnabled;
        this.smtpUsername = smtpUsername;
        this.smtpPassword = smtpPassword;
        this.logger = logger;
    }

    public async Task SendEmail(
        string message,
        string name,
        string email,
        string? phoneNumber,
        string? subject,
        bool? privacyPolicyAccepted,
        bool? dataProcessingAccepted)
    {
        try
        {
            logger.LogInformation("Sending mail from {name} ({email}, phone: {phoneNumber})", email, name, phoneNumber);
            var msg = new MimeMessage();
            msg.From.Add(MailboxAddress.Parse(administratorEmail));
            msg.To.Add(MailboxAddress.Parse(administratorEmail));
            msg.Subject = $"Message from website {websiteName}";
            var sb = new StringBuilder();
            sb.AppendLine($"Message from: {name}, {email}");
            if (!string.IsNullOrWhiteSpace(phoneNumber))
            {
                sb.AppendLine($"Phone number: {phoneNumber}");
            }
            if (!string.IsNullOrEmpty(subject))
            {
                sb.AppendLine($"Subject: {subject}");
            }
            if (privacyPolicyAccepted == true)
            {
                sb.AppendLine($"Sender has accepted privacy policy");
            }
            else
            {
                sb.AppendLine($"Sender has NOT accepted privacy policy!");
            }
            if (dataProcessingAccepted == true)
            {
                sb.AppendLine($"Sender has accepted data processing");
            }
            else
            {
                sb.AppendLine($"Sender has NOT accepted data processing!");
            }
            sb.AppendLine();
            sb.AppendLine(message);
            msg.Body = new TextPart("plain") { Text = sb.ToString() };
            using var client = new SmtpClient();
            await client.ConnectAsync(smtpServer, smtpPort, sslEnabled);
            await client.AuthenticateAsync(smtpUsername, smtpPassword);
            await client.SendAsync(msg);
            await client.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed sending email");
            throw;
        }
    }
}
