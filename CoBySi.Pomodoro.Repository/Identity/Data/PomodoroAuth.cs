using AspNetCore.Identity.CosmosDb;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CoBySi.Pomodoro.Repository.Identity.Data;

public class PomodoroAuth : CosmosIdentityDbContext<PomodoroUser, IdentityRole, string>
{
    public PomodoroAuth(DbContextOptions<PomodoroAuth> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<IdentityRole>()
        .Property(b => b.ConcurrencyStamp)
               .IsETagConcurrency();

        builder.Entity<PomodoroUser>()
        .Property(b => b.ConcurrencyStamp)
               .IsETagConcurrency();

        var index = builder.Entity<PomodoroUser>()
            .HasIndex(u => new { u.NormalizedEmail }).Metadata;
        var applicationUserType = builder.Entity<PomodoroUser>().Metadata.RemoveIndex(index.Properties);


        var userNameIndex = builder.Entity<PomodoroUser>()
            .HasIndex(u => new { u.NormalizedUserName }).Metadata;
        builder.Entity<PomodoroUser>().Metadata.RemoveIndex(userNameIndex.Properties);


        var identityRoleNameIndex = builder.Entity<IdentityRole>()
               .HasIndex(u => new { u.NormalizedName }).Metadata;
        builder.Entity<IdentityRole>().Metadata.RemoveIndex(identityRoleNameIndex.Properties);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
