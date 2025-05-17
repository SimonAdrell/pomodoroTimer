using CoBySi.Pomodoro.Repository.Identity.Data;
using CoBySi.Pomodoro.Web.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using Serilog;

namespace CoBySi.Pomodoro.Web.EmailService;

public class EmailSender : IEmailSender<PomodoroUser>
{
    private readonly EmailSettings _emailSettings;

    public EmailSender(IOptions<EmailSettings> optionsAccessor)
    {
        _emailSettings = optionsAccessor.Value;
        ArgumentException.ThrowIfNullOrEmpty(_emailSettings.Key);
    }

    public async Task SendConfirmationLinkAsync(PomodoroUser user, string email, string confirmationLink)
    {
        ArgumentException.ThrowIfNullOrEmpty(email);
        ArgumentException.ThrowIfNullOrEmpty(user.UserName);
        ArgumentException.ThrowIfNullOrEmpty(_emailSettings.ConfirmationTemplateId);

        await SendEmailAsync(email, user.UserName, _emailSettings.ConfirmationTemplateId, "Confirm your email!", new { link = confirmationLink });
        Log.Information("Confirmation email sent");
    }

    public async Task SendPasswordResetCodeAsync(PomodoroUser user, string email, string resetCode)
    {
        ArgumentException.ThrowIfNullOrEmpty(email);
        ArgumentException.ThrowIfNullOrEmpty(user.UserName);
        ArgumentException.ThrowIfNullOrEmpty(_emailSettings.ResetCodeTemplateId);

        await SendEmailAsync(email, user.UserName, _emailSettings.ResetCodeTemplateId, "Password reset code", new { link = resetCode });
    }

    public async Task SendPasswordResetLinkAsync(PomodoroUser user, string email, string resetLink)
    {
        ArgumentException.ThrowIfNullOrEmpty(email);
        ArgumentException.ThrowIfNullOrEmpty(user.UserName);
        ArgumentException.ThrowIfNullOrEmpty(_emailSettings.ResetLinkTemplateId);

        await SendEmailAsync(email, user.UserName, _emailSettings.ResetLinkTemplateId, "Password reset link", new { link = resetLink });
    }

    private async Task SendEmailAsync(string email, string name, string templateId, string Subject, object dynamicTemplateData)
    {
        var client = new SendGridClient(_emailSettings.Key);
        var from = new EmailAddress(_emailSettings.FromEmail, _emailSettings.FromName);
        var to = new EmailAddress(email, name);
        var msg = MailHelper.CreateSingleTemplateEmail(from, to, templateId, dynamicTemplateData);
        msg.Subject = Subject;
        msg.SetClickTracking(false, false);
        var response = await client.SendEmailAsync(msg);

        if (response.StatusCode != System.Net.HttpStatusCode.OK &&
            response.StatusCode != System.Net.HttpStatusCode.Accepted)
        {
            throw new InvalidOperationException($"Failed to send email: {response.StatusCode}");
        }
    }
}
