using LagalerieFurniture.Components;
using LagalerieFurniture.Data;
using LagalerieFurniture.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using MudBlazor;

var builder = WebApplication.CreateBuilder(args);

// أداة CLI لتشفير كلمات المرور الموجودة (تشغيل لمرة واحدة):
//   dotnet run -- migrate-passwords
if (args.Length > 0 && args[0] == "migrate-passwords")
{
    var exitCode = await LagalerieFurniture.Tools.PasswordMigrationTool.RunAsync(args);
    Environment.Exit(exitCode);
}

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Required for accessing HttpContext (client IP, cookie auth) inside services
builder.Services.AddHttpContextAccessor();

// Authentication & Authorization (Cookie-based)
builder.Services.AddAuthentication("LagalerieCookie")
    .AddCookie("LagalerieCookie", options =>
    {
        options.Cookie.Name = "LagalerieAuth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true;
        options.LoginPath = "/login";
        options.LogoutPath = "/logout";
        options.AccessDeniedPath = "/access-denied";
    });
builder.Services.AddAuthorization();

// Database - with retry on failure for remote connections
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null)
    )
);

// MudBlazor Theme Configuration
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;
    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = true;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 4000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

// Custom MudTheme - Gold & Dark (Matches Login Page)
builder.Services.AddScoped(sp => new MudTheme()
{
    PaletteLight = new PaletteLight()
    {
        Primary = "#d4af37",
        Secondary = "#6C3CE1",
        Background = "#f8fafc",
        Surface = "#ffffff",
        TextPrimary = "#1e293b",
        TextSecondary = "#64748b",
        AppbarBackground = "#ffffff",
        DrawerBackground = "#ffffff",
        DrawerText = "#1e293b",
        Success = "#10b981",
        Warning = "#f59e0b",
        Error = "#ef4444"
    },
    PaletteDark = new PaletteDark()
    {
        Primary = "#d4af37",
        Secondary = "#6C3CE1",
        Background = "#0b1120",
        Surface = "#151e32",
        TextPrimary = "#f1f5f9",
        TextSecondary = "#94a3b8",
        AppbarBackground = "#151e32",
        DrawerBackground = "#151e32",
        DrawerText = "#f1f5f9",
        Success = "#10b981",
        Warning = "#f59e0b",
        Error = "#ef4444"
    },
    LayoutProperties = new LayoutProperties()
    {
        DefaultBorderRadius = "12px",
        AppbarHeight = "64px"
    },
    Typography = new Typography()
    {
        Default = new DefaultTypography()
        {
            FontFamily = new[] { "Cairo", "Segoe UI", "sans-serif" },
            FontSize = "0.95rem",      // ✅ string
            FontWeight = "400",         // ✅ string (كان 400 int)
            LineHeight = "1.6",         // ✅ string (كان 1.6 double)
            LetterSpacing = "normal",
            TextTransform = "none"
        }
    }
});

// Authentication State Provider for Blazor circuits
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<CustomAuthStateProvider>());

// Auth Service
builder.Services.AddScoped<IAuthService, AuthService>();

// Password Hasher (BCrypt)
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

// Theme Service (for saving user preferences)
builder.Services.AddScoped<ThemeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();