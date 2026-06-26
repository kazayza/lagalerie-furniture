namespace LagalerieFurniture.Services;

public interface IAuthService
{
    Task<(bool Success, string? ErrorMessage, string? Username, string? Role)> LoginAsync(string username, string password);
    Task LogoutAsync();
    Task<string?> GetCurrentUserAsync();
}
