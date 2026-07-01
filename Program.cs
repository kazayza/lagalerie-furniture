using System.Globalization;
using System.Security.Claims;
using LagalerieFurniture.Components;
using LagalerieFurniture.Data;
using LagalerieFurniture.Models;
using LagalerieFurniture.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Localization;
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

// Cascading authentication state for Blazor Server
builder.Services.AddCascadingAuthenticationState();

// ============================================================
// Globalization & RTL — ضبط اللغة العربية والاتجاه من اليمين
// ============================================================
var supportedCultures = new[]
{
    new CultureInfo("ar-EG"),
    new CultureInfo("en-US")
};
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("ar-EG");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
    options.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());
});

// إعداد CultureInfo الافتراضي للتطبيق كله (decimal/dates)
CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("ar-EG");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("ar-EG");

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
        options.Cookie.SecurePolicy = builder.Environment.IsDevelopment()
            ? CookieSecurePolicy.SameAsRequest
            : CookieSecurePolicy.Always;
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
// نستخدم AddDbContextFactory (مسجّل كـ Singleton) عشان كل عملية DB تـ create
// DbContext مستقل. ده ضروري في Blazor Server عشان نتجنب مشكلة
// "A second operation was started on this context instance" اللي بتحصل
// لما الـ components بتـ share نفس الـ Scoped DbContext بشكل متوازي.
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null)
    ),
    ServiceLifetime.Scoped);

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
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<UserInfoService>();

// Authorization policy provider + handler (dynamic permission policies)
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

// تفعيل Localization قبل كل الـ middlewares التانية
app.UseRequestLocalization();

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
    if (!string.IsNullOrWhiteSpace(user.AvatarUrl))
{
    claims.Add(new Claim("avatar", user.AvatarUrl));
}

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
// لا نستخدم RequireAuthorization هنا: حتى لو المستخدم غير مسجل، نمسح أي cookie قديمة ونرجعه للـ login.
app.MapGet("/api/auth/logout", async (HttpContext context) =>
{
    await context.SignOutAsync("LagalerieCookie");

    context.Response.Cookies.Delete("LagalerieAuth", new CookieOptions
    {
        HttpOnly = true,
        Secure = !app.Environment.IsDevelopment(),
        SameSite = SameSiteMode.Lax,
        Path = "/"
    });

    return Results.Redirect("/login");
});

// ============================================================
// GET /api/auth/me — يرجّع بيانات المستخدم الحالي من الـ cookie
// ضروري لـ Blazor Server circuit حيث HttpContext = null
// ============================================================
app.MapGet("/api/auth/me", async (
    HttpContext context,
    ApplicationDbContext db,
    IPermissionService permissionService,
    ILogger<Program> logger) =>
{
    var user = context.User;
    if (user?.Identity?.IsAuthenticated != true)
    {
        logger.LogDebug("/api/auth/me: not authenticated");
        return Results.Unauthorized();
    }

    var userIdStr = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (!int.TryParse(userIdStr, out var userId))
    {
        logger.LogWarning("/api/auth/me: invalid userId claim = {Claim}", userIdStr);
        return Results.Unauthorized();
    }

    // نرجّع الـ user info + كل الـ claims
    var claims = user.Claims.Select(c => new { c.Type, c.Value }).ToList();
    var permissionCodes = await permissionService.GetEffectivePermissionCodesAsync(userId);

    logger.LogDebug("/api/auth/me: returning {ClaimCount} claims for userId={UserId}",
        claims.Count, userId);

    return Results.Ok(new
    {
        UserId = userId,
        Username = user.FindFirst(ClaimTypes.Name)?.Value,
        DisplayName = user.FindFirst(ClaimTypes.GivenName)?.Value,
        Email = user.FindFirst(ClaimTypes.Email)?.Value,
        Role = user.FindFirst(ClaimTypes.Role)?.Value,
AvatarUrl = user.FindFirst("avatar")?.Value,
Claims = claims,
Permissions = permissionCodes
    });
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