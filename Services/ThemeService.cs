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
        var preference = await _js.InvokeAsync<string>("localStorage.getItem", StorageKey);
        return preference != "light";
    }

    public async Task SetDarkModeAsync(bool isDark)
    {
        await _js.InvokeVoidAsync("localStorage.setItem", StorageKey, isDark ? "dark" : "light");
    }
}