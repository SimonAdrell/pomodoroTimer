using CoBySi.Pomodoro.Repository.Models;

namespace CoBySi.Pomodoro.Repository.Repositories;

public interface IPomodorosRepository
{
    Task<PomodoroEntity?> SavePomodoroItemAsync(PomodoroEntity pomodoroItem, CancellationToken cancellationToken);
}
