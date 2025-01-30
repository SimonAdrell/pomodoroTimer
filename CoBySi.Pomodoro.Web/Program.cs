using CoBySi.Pomodoro;
using CoBySi.Pomodoro.Repository;
using CoBySi.Pomodoro.Web.Components;
using Serilog;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CoBySi.Pomodoro.Repository.Identity.Data;
using CoBySi.Pomodoro.Web.Components.Account;
using Microsoft.AspNetCore.Components.Authorization;
using CoBySi.Pomodoro.Web.EmailService;
using CoBySi.Pomodoro.Web.Settings;
using CoBySi.Pomodoro.Repository.settings;
using CoBySi.Pomodoro.Web.Services;
using CoBySi.Pomodoro.Web.Cache;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("postgress")
    ?? throw new InvalidOperationException("Connection string 'postgress' not found."); ;

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
builder.Services.AddSingleton<IPomodoroSettingsService, PomodoroSettingsService>();

builder.Services.Configure<PomodoroSettings>(builder.Configuration.GetSection("PomodoroSettings"));

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<MongoSettiings>(builder.Configuration.GetSection("mongodb"));
builder.Services.Configure<RedisSettings>(builder.Configuration.GetSection("redis"));

builder.Services.AddSingleton<ISettingsCache, SettingsCache>();

builder.Services.AddSingleton(TimeProvider.System);

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddScoped<IdentityUserAccessor>();

builder.Services.AddScoped<IdentityRedirectManager>();

builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddStackExchangeRedisCache(options =>
 {
     var redisSettings = new RedisSettings();
     builder.Configuration.GetSection("redis").Bind(redisSettings);

     options.Configuration = redisSettings?.ConnectionString;
     options.InstanceName = redisSettings?.InstanceName;
 });

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
