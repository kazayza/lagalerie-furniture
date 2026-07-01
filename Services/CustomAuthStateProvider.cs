using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;

namespace LagalerieFurniture.Services;

/// <summary>
/// مزوّد حالة المصادقة لـ Blazor Server.
///
/// المشكلة الأساسية في Blazor Server:
///   HttpContext متاح بس في الـ pre-render phase (أول HTTP request).
///   بعد ما الـ WebSocket circuit يبدأ، HttpContextAccessor.HttpContext بيرجّع null.
///   فلو قعدنا نقرأ من HttpContext في كل مرة، الـ auth state هيبقى anonymous
///   بعد أول render.
///
/// الحل المتبع هنا:
///   1. في pre-render: نقرأ من HttpContext ونعمل cache للـ user في field
///   2. في circuit: HttpContext = null، فبنستخدم الـ cache
///   3. لو الـ cache فاضي (يعني الـ service اتعمل instance جديد في circuit)،
///      بنقرأ من HttpContext تاني (لو متاح) أو نرجّع anonymous
///
/// ملاحظة: الـ service Scoped، فالـ instance بيتعمل مرة واحدة لكل circuit.
/// لكن في pre-render بيكون instance مختلف. عشان كده الـ cache مش بيـ share
/// بين pre-render و circuit.
///
/// عشان نضمن إن الـ user متاح في الـ circuit، الـ MainLayout بيعمل
/// NavigateTo("/login", forceLoad: true) لو مش authenticated — ده بيكسر
/// الـ circuit ويعمل HTTP request جديد فيه HttpContext متاح.
/// </summary>
public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<CustomAuthStateProvider> _logger;

    // cache محلي طول الـ circuit
    private ClaimsPrincipal? _cachedUser;
    private bool _cacheInitialized = false;

    public CustomAuthStateProvider(
        IHttpContextAccessor httpContextAccessor,
        ILogger<CustomAuthStateProvider> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // 1) لو عندنا cached user، نستخدمه طول الـ circuit
        if (_cacheInitialized && _cachedUser?.Identity?.IsAuthenticated == true)
        {
            return Task.FromResult(new AuthenticationState(_cachedUser));
        }

        // 2) محاولة قراءة من HttpContext (متاح في pre-render وأي HTTP request جديد)
        var httpContextUser = _httpContextAccessor.HttpContext?.User;
        if (httpContextUser?.Identity?.IsAuthenticated == true)
        {
            // cache للـ user عشان نستخدمه بعدين في الـ circuit
            _cachedUser = httpContextUser;
            _cacheInitialized = true;

            _logger.LogDebug(
                "GetAuthenticationStateAsync: cached user {Username} with {ClaimCount} claims",
                httpContextUser.Identity?.Name,
                httpContextUser.Claims.Count());

            return Task.FromResult(new AuthenticationState(httpContextUser));
        }

        // 3) anonymous — بس نـ cache ده برضه عشان ما نعيدش المحاولة في كل call
        if (!_cacheInitialized)
        {
            _cachedUser = new ClaimsPrincipal(new ClaimsIdentity());
            _cacheInitialized = true;
        }

        _logger.LogDebug("GetAuthenticationStateAsync: anonymous (HttpContext null={IsNull})",
    _httpContextAccessor.HttpContext == null);

var anonymousUser = _cachedUser ?? new ClaimsPrincipal(new ClaimsIdentity());
return Task.FromResult(new AuthenticationState(anonymousUser));
    }

    /// <summary>
    /// إجبار الـ provider على إعادة قراءة الـ auth state في الـ call الجاي.
    /// مفيد لما المستخدم يعمل login/logout.
    /// </summary>
    public void MarkUserAsAuthenticated(string username, string role = "User")
    {
        // امسح الـ cache
        _cachedUser = null;
        _cacheInitialized = false;

        // اعمل user مؤقت عشان الـ Blazor UI يتحدث
        var identity = new ClaimsIdentity(
            new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role),
            },
            "LagalerieCookie");

        var principal = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
    }

    public void MarkUserAsLoggedOut()
    {
        // امسح الـ cache
        _cachedUser = null;
        _cacheInitialized = false;

        var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
    }

    /// <summary>
    /// يجبر الـ provider على إعادة قراءة الـ cookie في الـ render الجاي.
    /// </summary>
    public void Refresh()
    {
        _cachedUser = null;
        _cacheInitialized = false;

        var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
    }
}