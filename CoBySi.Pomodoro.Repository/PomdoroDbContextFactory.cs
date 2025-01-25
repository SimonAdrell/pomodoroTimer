using CoBySi.Pomodoro.Repository.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CoBySi.Pomodoro.Repository;

public class PomdoroDbContextFactory : IDesignTimeDbContextFactory<PomodoroAuth>
{
    public PomdoroDbContextFactory()
    {
    }

    public PomodoroAuth CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PomodoroAuth>();
        optionsBuilder.UseNpgsql("User ID=postgres;Password=YourStrong!Passw0rd;Host=localhost;Port=5432;Database=pomodoro;Pooling=true;MinPoolSize=0;MaxPoolSize=100;Connection Lifetime=0;");

        return new PomodoroAuth(optionsBuilder.Options);
    }
}
