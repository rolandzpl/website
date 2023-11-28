namespace website.Services;

public interface IEmailService
{
    Task SendEmail(string message, string name, string email, string? phoneNumber, string? subject, bool? privacyPolicyAccepted, bool? dataProcessingAccepted);
}
