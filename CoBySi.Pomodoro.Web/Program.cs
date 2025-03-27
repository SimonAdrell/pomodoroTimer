using CoBySi.Pomodoro;
using CoBySi.Pomodoro.Repository;
using CoBySi.Pomodoro.Web.Components;
using Serilog;
using Microsoft.AspNetCore.Identity;
using CoBySi.Pomodoro.Repository.Identity.Data;
using CoBySi.Pomodoro.Web.Components.Account;
using Microsoft.AspNetCore.Components.Authorization;
using CoBySi.Pomodoro.Web.EmailService;
using CoBySi.Pomodoro.Web.Settings;
using CoBySi.Pomodoro.Web.Services;
using CoBySi.Pomodoro.Web.Cache;
using CoBySi.Pomodoro.Repository.Repositories;
using AspNetCore.Identity.CosmosDb.Extensions;
using CoBySi.Pomodoro.Repository.settings;

var builder = WebApplication.CreateBuilder(args);


Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateLogger();

Log.Information("Starting {application}", "CoBySi.Pomodoro.Web");

await builder.AddCosmosDb();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<IPomodorHandler, PomodorHandler>();

builder.Services.AddSingleton<ISettingsRepository, SettingsRepository>();
builder.Services.Decorate<ISettingsRepository, CacheSettingsRepository>();

builder.Services.AddSingleton<ISettingsService, SettingsService>();
builder.Services.AddSingleton<ILocalStorageService, LocalStorageService>();

builder.Services.Configure<PomodoroSettings>(builder.Configuration.GetSection("PomodoroSettings"));
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<RedisSettings>(builder.Configuration.GetSection("redis"));

builder.Services.AddSingleton(
        builder.Configuration.GetSection("SettingsDbSettings").Get<SettingsDbSettings>() ??
            throw new NullReferenceException());


builder.Services.AddSingleton<ISettingsCache, SettingsCache>();

builder.Services.AddSingleton(TimeProvider.System);

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddScoped<IdentityUserAccessor>();

builder.Services.AddScoped<IdentityRedirectManager>();

builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();

builder.Services.AddScoped<INotificationService, NotificationService>();

builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddStackExchangeRedisCache(options =>
 {
     var redisSettings = new RedisSettings();
     builder.Configuration.GetSection("redis").Bind(redisSettings);

     options.Configuration = redisSettings?.ConnectionString;
     options.InstanceName = redisSettings?.InstanceName;
 });


builder.Services.AddCosmosIdentity<PomodoroAuth, PomodoroUser, IdentityRole, string>(
      options => options.SignIn.RequireConfirmedAccount = true
    )
    .AddDefaultUI() // Use this if Identity Scaffolding is in use
    .AddEntityFrameworkStores<PomodoroAuth>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddBlazorBootstrap();

builder.Services.AddSingleton<IEmailSender<PomodoroUser>, EmailSender>();

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
