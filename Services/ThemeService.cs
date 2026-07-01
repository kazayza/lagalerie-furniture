using Microsoft.JSInterop;

namespace LagalerieFurniture.Services;

public class ThemeService
{
    private readonly IJSRuntime _js;
    private const string StorageKey = "lagalerie_theme_preference";

    public ThemeService(IJSRuntime js)
    {
        _js = js;
    }

    public async Task<bool> IsDarkModeAsync()
    {
        try
        {
            var preference = await _js.InvokeAsync<string>("localStorage.getItem", StorageKey);
            return preference != "light";
        }
        catch
        {
            return true;
        }
    }

    public async Task SetDarkModeAsync(bool isDark)
    {
        await _js.InvokeVoidAsync("localStorage.setItem", StorageKey, isDark ? "dark" : "light");
    }

    /// <summary>
    /// يضيف class واضح على html/body عشان CSS variables تتغير فوراً
    /// حتى لو MudBlazor class اتأخر أو اتغير مكانه.
    /// </summary>
    public async Task ApplyThemeClassAsync(bool isDark)
    {
        var theme = isDark ? "dark" : "light";
        await _js.InvokeVoidAsync("lagalerieTheme.apply", theme);
    }
}
