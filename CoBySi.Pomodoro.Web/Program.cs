using CoBySi.Pomodoro;
using CoBySi.Pomodoro.Repository;
using CoBySi.Pomodoro.Web.Components;
using CoBySi.Pomodoro.Web.PomodoroProperties;
using Serilog;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CoBySi.Pomodoro.Repository.Identity.Data;
using CoBySi.Pomodoro.Web.Components.Account;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("postgress") ?? throw new InvalidOperationException("Connection string 'postgress' not found."); ;

builder.Services.AddDbContext<PomodoroAuth>(options => options.UseNpgsql(connectionString));

// builder.Services.AddDefaultIdentity<PomodoroUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<PomodoroAuth>();
Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateLogger();

Log.Information("Starting {application}", "CoBySi.Pomodoro.Web");

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<IPomodorHandler, PomodorHandler>();
builder.Services.AddSingleton<IUserSettingsRepository, UserSettingsRepository>();
builder.Services.AddSingleton<IPomdoroPropertiesHandler, PomdoroPropertiesHandler>();

var confg = builder.Configuration;
builder.Services.AddSingleton(sp =>
{
    var settings = new PomodoroSettings();
    confg.GetSection("PomodoroSettings").Bind(settings);
    return settings;
});

builder.Services.AddSingleton(TimeProvider.System);

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddScoped<IdentityUserAccessor>();

builder.Services.AddScoped<IdentityRedirectManager>();

builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

builder.Services.AddIdentityCore<PomodoroUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<PomodoroAuth>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<PomodoroUser>, IdentityNoOpEmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapAdditionalIdentityEndpoints(); ;

app.Run();
