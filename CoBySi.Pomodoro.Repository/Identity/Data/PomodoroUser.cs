using Microsoft.AspNetCore.Identity;

namespace CoBySi.Pomodoro.Repository.Identity.Data;

public class PomodoroUser : IdentityUser
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

