namespace LagalerieFurniture.Models;

/// <summary>
/// DTO لطلب إنشاء login token مؤقت.
/// يُرسل من Login.razor إلى POST /api/auth/token
/// </summary>
public record LoginTokenRequest(string Username, string? Role);
