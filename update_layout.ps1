$path = "e:\ERP_Furniture\lagalerie-furnitureBlazor\LagalerieFurniture\Components\Layout\MainLayout.razor"
$newContent = @"
@using MudBlazor
@using LagalerieFurniture.Services
@using System.Linq
@using System.Security.Claims
@inject IAuthService AuthService
@inject NavigationManager Navigation
@inject AuthenticationStateProvider AuthenticationStateProvider
@inject ThemeService ThemeService
@inject IMenuService MenuService
@inherits LayoutComponentBase

<MudThemeProvider @bind-IsDarkMode="_isDarkMode" />
<MudPopoverProvider />
<MudDialogProvider />
<MudSnackbarProvider />

<MudLayout>
    @* الهيدر العلوي *@
    <TopHeader ToggleDrawerCallback="ToggleDrawer" />

    @* القائمة الجانبية على اليمين (Anchor.End في RTL = يمين) *@
    <MudDrawer @bind-Open="_drawerOpen"
               Variant="DrawerVariant.Responsive"
               ClipMode="DrawerClipMode.Always"
               Elevation="0"
               Anchor="Anchor.End"
               Class="glass-drawer">
        <SidebarMenu @bind-IsMiniMode="_isMiniMode" />
    </MudDrawer>

    <MudMainContent Class="main-content-area"
                     Style="background: var(--bg-dark); min-height: 100vh;">
        <div class="main-content-inner @(_isMiniMode ? "sidebar-mini-active" : "")">
            <div class="page-header">
                <div class="page-header-greeting">
                    <MudText Typo="Typo.h5" Style="font-weight: 700; color: var(--text-primary);">
                        @_greetingText
                    </MudText>
                    <MudText Typo="Typo.body2" Color="Color.Secondary">
                        @_currentDate
                    </MudText>
                </div>
            </div>
            <MudContainer MaxWidth="MaxWidth.ExtraExtraLarge" Class="pa-6 fade-in">
                @Body
            </MudContainer>
        </div>
    </MudMainContent>
</MudLayout>

@code {
    private string _currentUsername = string.Empty;
    private string _userRole = string.Empty;
    private bool _drawerOpen = true;
    private bool _isDarkMode = true;
    private bool _isMiniMode = false;
    private string _greetingText = "👋 مرحباً بك";
    private string _currentDate = "";

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (!(user.Identity?.IsAuthenticated ?? false))
        {
            Navigation.NavigateTo("/login", true);
            return;
        }

        _currentUsername = user.FindFirst(ClaimTypes.GivenName)?.Value
                         ?? user.Identity?.Name
                         ?? "مستخدم";
        _userRole = user.FindFirst(ClaimTypes.Role)?.Value ?? "مستخدم";

        var hour = DateTime.Now.Hour;
        _greetingText = (hour switch
        {
            >= 5 and < 12 => "🌅 صباح الخير";
            >= 12 and < 17 => "🌤️ مساء الخير";
            _ => "🌙 تصبح على خير";
        }) + $"، {_currentUsername}";

        _currentDate = DateTime.Now.ToString("dddd، d MMMM yyyy");
        try { _isMiniMode = await MenuService.GetSidebarMiniModeAsync(); } catch { }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _isDarkMode = await ThemeService.IsDarkModeAsync();
            StateHasChanged();
        }
    }

    private void ToggleDrawer() => _drawerOpen = !_drawerOpen;
}
"@
Set-Content -Path $path -Value $newContent -Encoding UTF8
Write-Host "Done"
