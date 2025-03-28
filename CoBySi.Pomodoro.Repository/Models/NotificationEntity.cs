namespace CoBySi.Pomodoro.Repository.Models;

public class NotificationEntity
{
    public bool NotificationEnabled { get; set; }
    public bool NotificationSoundEnabled { get; set; }
    public int SoundID { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastChanged { get; set; }
}
