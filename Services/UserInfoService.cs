using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace LagalerieFurniture.Services;

/// <summary>
/// يجيب بيانات المستخدم الحالي من /api/auth/me endpoint.
///
/// ضروري في Blazor Server لأن HttpContext بيرجّع null بعد ما الـ circuit يبدأ،
/// فالـ CustomAuthStateProvider مش هيلاقي الـ claims. الـ service ده بيعمل
/// HTTP call للـ endpoint (اللي بيتعامل مع HttpContext على السيرفر) ويـ cache
/// النتيجة طول الـ circuit.
/// </summary>
public class UserInfoService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<UserInfoService> _logger;
    private readonly NavigationManager _navigation;

    // cache طول الـ circuit
    private UserInfoDto? _cached;
    private bool _loaded = false;

    public UserInfoService(
        HttpClient httpClient,
        NavigationManager navigation,
        ILogger<UserInfoService> logger)
    {
        _httpClient = httpClient;
        _navigation = navigation;
        _logger = logger;
    }

    public async Task<UserInfoDto?> GetCurrentUserAsync(bool forceRefresh = false)
    {
        if (_loaded && !forceRefresh && _cached != null)
            return _cached;

        try
        {
            var url = $"{_navigation.BaseUri}api/auth/me";
            _logger.LogDebug("UserInfoService: calling {Url}", url);

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("UserInfoService: /api/auth/me returned {Status}", response.StatusCode);
                _cached = null;
                _loaded = true;
                return null;
            }

            _cached = await response.Content.ReadFromJsonAsync<UserInfoDto>();
            _loaded = true;

            _logger.LogInformation(
                "UserInfoService: loaded user {Username} (id={UserId}, role={Role}, {PermCount} permissions, {ClaimCount} claims)",
                _cached?.Username, _cached?.UserId, _cached?.Role,
                _cached?.Permissions?.Count ?? 0, _cached?.Claims?.Count ?? 0);

            return _cached;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UserInfoService: failed to fetch /api/auth/me");
            _cached = null;
            _loaded = true;
            return null;
        }
    }

    public async Task<int> GetCurrentUserIdAsync()
    {
        var user = await GetCurrentUserAsync();
        return user?.UserId ?? 0;
    }

    public async Task<HashSet<string>> GetPermissionsAsync()
    {
        var user = await GetCurrentUserAsync();
        if (user?.Permissions == null) return new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        return new HashSet<string>(user.Permissions, StringComparer.OrdinalIgnoreCase);
    }

    public async Task<bool> IsAdminAsync()
    {
        var user = await GetCurrentUserAsync();
        if (user == null) return false;
        return string.Equals(user.Role, "Admin", StringComparison.OrdinalIgnoreCase)
            || string.Equals(user.Role, "SuperAdmin", StringComparison.OrdinalIgnoreCase);
    }

    public void ClearCache()
    {
        _cached = null;
        _loaded = false;
    }
}

/// <summary>DTO لاستجابة /api/auth/me.</summary>
public class UserInfoDto
{
    public int UserId { get; set; }
    public string? Username { get; set; }
    public string? DisplayName { get; set; }
    public string? Email { get; set; }
    public string? Role { get; set; }
    public List<ClaimDto> Claims { get; set; } = new();
    public HashSet<string> Permissions { get; set; } = new(StringComparer.OrdinalIgnoreCase);
}

public class ClaimDto
{
    public string Type { get; set; } = "";
    public string Value { get; set; } = "";
}