using CoBySi.Pomodoro.Repository.Models;

namespace CoBySi.Pomodoro.Repository.Repositories;

public class PomodorosRepository : CosmosRepositoryBase<PomodoroEntity>, IPomodorosRepository
{
    public PomodorosRepository(CosmosBaseSettings settings) : base(settings)
    {
    }

    public async Task<PomodoroEntity?> SavePomodoroItemAsync(PomodoroEntity pomodoroItem,
        CancellationToken cancellationToken)
    {
        return await Upsert(pomodoroItem, cancellationToken);
    }
}
