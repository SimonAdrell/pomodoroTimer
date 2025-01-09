using CoBySi.Pomodoro;
using CoBySi.Pomodoro.Repository;
using CoBySi.Pomodoro.Web.Components;
using CoBySi.Pomodoro.Web.PomodoroProperties;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
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
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
