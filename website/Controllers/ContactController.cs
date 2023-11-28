using Microsoft.AspNetCore.Mvc;
using website.Services;

namespace Lithium.Website.Controllers;

[ApiController]
[Route("[controller]")]
public class ContactController : ControllerBase
{
    private readonly IEmailService emailService;

    public ContactController(IEmailService emailService)
    {
        this.emailService = emailService;
    }

    [HttpPost("/api/contact")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task SendEmail(
        [FromForm] string message,
        [FromForm] string name,
        [FromForm] string email,
        [FromForm] string? phoneNumber,
        [FromForm] string? subject,
        [FromForm] bool? privacyPolicyAccepted,
        [FromForm] bool? dataProcessingAccepted)
    {
        await emailService.SendEmail(message, name, email, phoneNumber, subject, privacyPolicyAccepted, dataProcessingAccepted);
    }
}
