using System.Security.Claims;
using LagalerieFurniture.Components;
using LagalerieFurniture.Data;
using LagalerieFurniture.Models;
using LagalerieFurniture.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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

// HttpClient for internal API calls (Login.razor → /api/auth/token)
builder.Services.AddHttpClient();

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
builder.Services.AddAuthorization(options =>
{
    // الـ PermissionPolicyProvider يوفّر policies لأي كود صلاحية تلقائياً
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
});

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
builder.Services.AddScoped<IMenuService, MenuService>();

// Permission Services
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserPermissionService, UserPermissionService>();

// Authorization policy provider + handler (dynamic permission policies)
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddScoped<PermissionAuthorizationHandler>();

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

// ============================================================
// Auth API Endpoints — Minimal API
// ============================================================
// لما الـ Blazor component يحتاج يعمل SignIn/SignOut:
// مينفعش يستخدم HttpContext.SignInAsync مباشرة (headers read-only
// بعد ما الـ WebSocket circuit يبدأ). فنعمل HTTP request جديد.
//
// الـ flow:
//   1. Login.razor يتأكد من الباسورد عبر AuthService.LoginAsync()
//   2. Login.razor ينشئ token مؤقت وينقّل لـ /api/auth/login
//   3. الـ endpoint يصدر cookie حقيقية ويرجّع لـ /
// ============================================================

// مخزّن مؤقت للـ tokens (in-memory, scoped للـ app lifetime)
// الـ token بيكون: username + timestamp + عشوائي — صالح 30 ثانية
var loginTokens = new Dictionary<string, (string Username, string Role, DateTime Expires)>();

// POST /api/auth/login?token=xxx
// يصدر الـ authentication cookie
app.MapGet("/api/auth/login", async (
    string token,
    HttpContext context,
    ApplicationDbContext db,
    IPasswordHasher passwordHasher,
    IPermissionService permissionService,
    ILogger<Program> logger) =>
{
    // تحقق من صلاحية الـ token
    if (!loginTokens.TryGetValue(token, out var tokenData))
    {
        logger.LogWarning("login endpoint: token غير صالح");
        return Results.Redirect("/login");
    }

    if (tokenData.Expires < DateTime.UtcNow)
    {
        loginTokens.Remove(token);
        logger.LogWarning("login endpoint: token منتهي الصلاحية");
        return Results.Redirect("/login");
    }

    loginTokens.Remove(token);

    // جلب المستخدم من الـ DB
    var user = await db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == tokenData.Username);
    if (user == null)
    {
        logger.LogError("login endpoint: المستخدم {Username} مش موجود", tokenData.Username);
        return Results.Redirect("/login");
    }

    // حساب الصلاحيات الفعّالة (Role + User overrides)
    var permissionCodes = await permissionService.GetEffectivePermissionCodesAsync(user.Id);

    // إصدار cookie
    var claims = new List<Claim>
    {
        new(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new(ClaimTypes.Name, user.Username),
        new(ClaimTypes.Email, user.Email),
        new(ClaimTypes.GivenName, user.DisplayName),
        new(ClaimTypes.Role, tokenData.Role),
    };

    // إضافة كل أكواد الصلاحيات كـ claims مستقلة (للـ AuthorizeView و policy provider)
    foreach (var code in permissionCodes)
    {
        claims.Add(new Claim("permission", code));
    }

    var identity = new ClaimsIdentity(claims, "LagalerieCookie");
    var principal = new ClaimsPrincipal(identity);

    await context.SignInAsync("LagalerieCookie", principal, new AuthenticationProperties
    {
        IsPersistent = true,
        ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7),
        AllowRefresh = true
    });

    logger.LogInformation("تم إصدار cookie للمستخدم: {Username}", tokenData.Username);
    return Results.Redirect("/");
});

// GET /api/auth/logout
// يمسح الـ cookie ويرجّع لصفحة الدخول
app.MapGet("/api/auth/logout", async (HttpContext context) =>
{
    await context.SignOutAsync("LagalerieCookie");
    return Results.Redirect("/login");
}).RequireAuthorization();

// POST /api/auth/token — ينشئ token مؤقت للاستخدام في الـ redirect
// بيتستدعى من الـ Login.razor بعد نجاح التحقق
app.MapPost("/api/auth/token", (LoginTokenRequest request, ILogger<Program> logger) =>
{
    if (string.IsNullOrWhiteSpace(request.Username))
        return Results.BadRequest(new { error = "اسم المستخدم مطلوب" });

    var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray())[..16]; // 16 حرف عشوائي
    loginTokens[token] = (request.Username, request.Role ?? "User", DateTime.UtcNow.AddSeconds(30));

    logger.LogDebug("تم إنشاء login token للمستخدم: {Username}", request.Username);
    return Results.Ok(new { token });
});

app.Run();