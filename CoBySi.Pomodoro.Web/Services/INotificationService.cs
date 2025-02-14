using System;
using System.Runtime.CompilerServices;
using CoBySi.Pomodoro.Models;
using CoBySi.Pomodoro.Repository.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace CoBySi.Pomodoro.Web.Services;

public interface INotificationService
{
    Task InvokeNotificationPermissionAsync();
    Task InvokeNotificaionShow(string title, string body, string iconUrl);
    Task PlayCompletionSoundAsync();
}
