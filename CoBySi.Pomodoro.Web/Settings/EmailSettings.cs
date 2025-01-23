using System;

namespace CoBySi.Pomodoro.Web.Settings;

public class EmailSettings
{
    public string? Key { get; set; }
    public string? FromEmail { get; set; }
    public string? FromName { get; set; }
    public string? ConfirmationTemplateId { get; set; }
    public string? ResetCodeTemplateId { get; set; }
    public string? ResetLinkTemplateId { get; set; }
}
